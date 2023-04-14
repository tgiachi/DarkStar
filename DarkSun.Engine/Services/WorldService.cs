using System.Collections.Concurrent;
using System.Diagnostics;
using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Data.Config;
using DarkSun.Api.Engine.Data.Config;
using DarkSun.Api.Engine.Events.Map;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Api.Engine.Map.Entities;
using DarkSun.Api.Engine.Map.Entities.Base;
using DarkSun.Api.Engine.Serialization;
using DarkSun.Api.Engine.Serialization.Map;
using DarkSun.Api.Engine.Utils;
using DarkSun.Api.World.Types.Map;
using DarkSun.Api.World.Types.Tiles;
using DarkSun.Database.Entities.Maps;
using DarkSun.Engine.Services.Base;
using DarkSun.Network.Protocol.Messages.Common;
using FastEnumUtility;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using Microsoft.Extensions.Logging;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace DarkSun.Engine.Services
{
    [DarkSunEngineService("WorldService", 10)]
    public class WorldService : BaseService<IWorldService>, IWorldService
    {
        private readonly EngineConfig _engineConfig;
        private readonly DirectoriesConfig _directoriesConfig;

        private readonly ConcurrentDictionary<string, (Map, MapType, MapInfo)> _maps = new();

        public WorldService(ILogger<WorldService> logger, EngineConfig engineConfig, DirectoriesConfig directoriesConfig) : base(logger)
        {
            _engineConfig = engineConfig;
            _directoriesConfig = directoriesConfig;
        }

        public override async ValueTask<bool> StopAsync()
        {
            // await Task.Delay(3000);
            await SaveMapsAsync();
            return true;
        }

        protected override async ValueTask<bool> StartAsync()
        {
            await GenerateMapsAsync();
            await SaveMapsAsync();
            Engine.JobSchedulerService.AddJob("SaveMaps",
                async () => { await SaveMapsAsync(); }, (int)TimeSpan.FromMinutes(_engineConfig.Maps.SaveEveryMinutes).TotalSeconds, false);

            return true;
        }



        private async ValueTask GenerateMapsAsync()
        {
            var mapsToGenerate = Enumerable.Range(1, _engineConfig.Maps.Cities.Num)
                .Select(_ => Task.Run(async () =>
                {
                    var (id, map) = await BuildMapAsync(MapType.City);
                    _maps.TryAdd(id, (map, MapType.City, new MapInfo()));
                }))
                .ToList();


            mapsToGenerate.AddRange(Enumerable.Range(1, _engineConfig.Maps.Dungeons.Num)
                .Select(_ => Task.Run(async () =>
                {
                    var (id, map) = await BuildMapAsync(MapType.Dungeon);
                    HandleMapEvents(id, map);
                    _maps.TryAdd(id, (map, MapType.Dungeon, new MapInfo()));
                }))
                .ToList());

            var mapGeneratingStopwatch = new Stopwatch();
            mapGeneratingStopwatch.Start();
            await Task.WhenAll(mapsToGenerate);
            await SaveMapsOnDbAsync();
            mapGeneratingStopwatch.Stop();

            Logger.LogInformation("Generated {NumMaps} maps in {Time}ms", _maps.Count, mapGeneratingStopwatch.ElapsedMilliseconds);
        }

        private async ValueTask SaveMapsOnDbAsync()
        {
            foreach (var maps in _maps)
            {
                var map = maps.Value;
                var mapId = maps.Key;
                await Engine.DatabaseService.InsertAsync<MapEntity>(new MapEntity()
                {
                    Name = map.Item3.Name,
                    MapId = mapId,
                    Type = map.Item2,
                    FileName = $"{mapId}.map"
                });
            }
        }

        private async ValueTask SaveMapsAsync()
        {
            var savingStopWatch = new Stopwatch();
            savingStopWatch.Start();
            Logger.LogInformation("Saving maps to file system");
            foreach (var map in _maps)
            {
                await SaveMapAsync(map.Key);
            }
            savingStopWatch.Stop();
            Logger.LogInformation("Saved {NumMaps} maps in {Time} ms", _maps.Count, savingStopWatch.ElapsedMilliseconds);
        }

        private async ValueTask SaveMapAsync(string mapId)
        {
            var map = _maps[mapId];
            var mapEntity = new MapObjectSerialization() { Name = map.Item3.Name, MapId = mapId, MapType = map.Item2, Height = map.Item1.Height, Width = map.Item1.Width };

            foreach (var terrainPosition in map.Item1.Terrain.Positions())
            {
                var terrainObject = map.Item1.GetTerrainAt(terrainPosition) as BaseGameObject;
                mapEntity.Layers.Add(new LayerObjectSerialization()
                {
                    Type = MapLayer.Terrain,
                    Tile = terrainObject!.Tile,
                    Position = new PointPosition(terrainPosition.X, terrainPosition.Y)
                });
            }

            foreach (var gameObject in map.Item1.Entities.Items)
            {
                var baseGameObject = gameObject as BaseGameObject;
                mapEntity.Layers.Add(new ()
                {
                    Type = (MapLayer)gameObject.Layer,
                    Tile = baseGameObject!.Tile,
                    ObjectId = baseGameObject.ObjectId,
                    Position = new (gameObject.Position.X, gameObject.Position.Y)
                });
            }


            await BinarySerialization.SerializeToFileAsync(mapEntity,
                Path.Join(_directoriesConfig[DirectoryNameType.Maps], $"{mapId}.map"));

        }

        private async Task<(string, Map)> BuildMapAsync(MapType mapType)
        {
            var id = GenerateMapId();
            Logger.LogDebug("Generating map type {MapType}", mapType);
            switch (mapType)
            {
                case MapType.City:
                    return (id, await GenerateCityMapAsync(id));
                case MapType.Dungeon:
                    return (id, await GenerateDungeonMapAsync(id));
            }

            throw new Exception($"Can't find map type generator {mapType}");
        }

        private ValueTask<Map> GenerateCityMapAsync(string id)
        {
            var cityMapGenerator = new Generator(_engineConfig.Maps.Cities.Width, _engineConfig.Maps.Cities.Height)
                .ConfigAndGenerateSafe(generator =>
                {
                    generator.AddSteps(DefaultAlgorithms.RectangleMapSteps());
                }, 3);

            var wallsFloors = cityMapGenerator.Context.GetFirst<ArrayView<bool>>("WallFloor");
            var map = new Map(_engineConfig.Maps.Cities.Width, _engineConfig.Maps.Cities.Height,
                FastEnum.GetValues<MapLayer>().Count, Distance.Chebyshev);

            map.ApplyTerrainOverlay(wallsFloors, (pos, val) => val
                ? new TerrainGameObject(pos) { IsWalkable = true, IsTransparent = true, Tile = TileType.Null }
                : new TerrainGameObject(pos, false, false) { Tile = TileType.Null });

            return ValueTask.FromResult(map);

        }

        private void HandleMapEvents(string id, Map map)
        {
            map.ObjectAdded += (_, args) =>
            {
                Logger.LogDebug("Added {GameObject} to map {MapId} Layer: {Layer}", args.Item, id, ((MapLayer)args.Item.Layer).FastToString());
                HandleGameObjectAdded(id, args.Item, args.Position.ToPointPosition());
            };
            map.ObjectMoved += (_, args) =>
            {
                Logger.LogDebug("Moved {GameObject} to map {MapId} Layer: {Layer}", args.Item, id, ((MapLayer)args.Item.Layer).FastToString());
                HandleGameObjectMoved(id, args.Item, args.OldPosition.ToPointPosition(), args.NewPosition.ToPointPosition());
            };
            map.ObjectRemoved += (_, args) =>
            {
                Logger.LogDebug("Removed {GameObject} to map {MapId} Layer: {Layer}", args.Item, id, ((MapLayer)args.Item.Layer).FastToString());
                HandleGameObjectRemoved(id, args.Item, args.Position.ToPointPosition());
            };

        }

        private void HandleGameObjectAdded(string mapId, IGameObject gameObject, PointPosition position)
        {
            var baseGameObject = gameObject as BaseGameObject;
            Engine.EventBus.PublishAsync(new GameObjectAddedEvent(mapId, (MapLayer)gameObject.Layer, position,
                baseGameObject!.ObjectId));
        }

        private void HandleGameObjectMoved(string mapId, IGameObject gameObject, PointPosition oldPosition, PointPosition newPosition)
        {
            var baseGameObject = gameObject as BaseGameObject;
            Engine.EventBus.PublishAsync(new GameObjectMovedEvent(mapId, (MapLayer)gameObject.Layer, oldPosition, newPosition,
                               baseGameObject!.ObjectId));
        }

        private void HandleGameObjectRemoved(string mapId, IGameObject gameObject, PointPosition position)
        {
            var baseGameObject = gameObject as BaseGameObject;
            Engine.EventBus.PublishAsync(new GameObjectRemovedEvent(mapId, (MapLayer)gameObject.Layer, position,
                               baseGameObject!.ObjectId));
        }

        private ValueTask<Map> GenerateDungeonMapAsync(string id)
        {
            var dungeonGenerator = new Generator(_engineConfig.Maps.Dungeons.Width, _engineConfig.Maps.Dungeons.Height)
                .ConfigAndGenerateSafe(generator =>
                {
                    generator.AddSteps(DefaultAlgorithms.DungeonMazeMapSteps());
                }, 3);

            var map = new Map(_engineConfig.Maps.Dungeons.Width, _engineConfig.Maps.Dungeons.Height,
                FastEnum.GetValues<MapLayer>().Count, Distance.Chebyshev);

            var wallsFloors = dungeonGenerator.Context.GetFirst<ArrayView<bool>>("WallFloor");

            map.ApplyTerrainOverlay(wallsFloors, (pos, val) => val
                ? new TerrainGameObject(pos) { IsWalkable = true, IsTransparent = true, Tile = TileType.Null }
                : new TerrainGameObject(pos, false, false) { Tile = TileType.Null });

            return ValueTask.FromResult(map);
        }

        private string GenerateMapId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
