using System.Collections.Concurrent;
using System.Diagnostics;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Data.Config;
using DarkStar.Api.Engine.Data.Config;
using DarkStar.Api.Engine.Events.Map;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.Engine.Map.Entities.Base;
using DarkStar.Api.Engine.Serialization;
using DarkStar.Api.Engine.Serialization.Map;
using DarkStar.Api.Engine.Utils;
using DarkStar.Api.Utils;
using DarkStar.Api.World.Types.Map;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Database.Entities.Maps;
using DarkStar.Engine.Services.Base;
using DarkStar.Network.Protocol.Messages.Common;

using FastEnumUtility;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using Microsoft.Extensions.Logging;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace DarkStar.Engine.Services
{
    [DarkStarEngineService("WorldService", 10)]
    public class WorldService : BaseService<IWorldService>, IWorldService
    {
        private readonly EngineConfig _engineConfig;
        private readonly DirectoriesConfig _directoriesConfig;
        private readonly Radius _positionRadius = new();

        private readonly ConcurrentDictionary<string, (Map map, MapType mapType, MapInfo mapInfo)> _maps = new();

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
                () =>
                {
                    _ = Task.Run(SaveMapsAsync);

                }, (int)TimeSpan.FromMinutes(_engineConfig.Maps.SaveEveryMinutes).TotalSeconds, false);

            return true;
        }

        public Task<Dictionary<MapLayer, List<IGameObject>>> GetGameObjectsInRangeAsync(string mapId,
            PointPosition position, int range = 5)
        {
            var map = GetMap(mapId);
            var gameObjects = FastEnum.GetValues<MapLayer>().ToDictionary(layer => layer, layer => new List<IGameObject>());

            var positionsInRadius = _positionRadius.PositionsInRadius(position.ToPoint(), range);
            foreach (var pos in positionsInRadius)
            {
                var foundObjects = map.Entities.GetItemsAt(pos);
                foreach (var gameObject in foundObjects)
                {
                    gameObjects[(MapLayer)gameObject.Layer].Add(gameObject);
                }
            }
            return Task.FromResult(gameObjects);
        }

        public Task<List<PointPosition>> GetNeighborCellsAsync(string mapId, PointPosition startPosition, int cellsNumber = 5)
        {
            var positions = new List<PointPosition>();
            var map = GetMap(mapId);
            var radiusResult = _positionRadius.PositionsInRadius(startPosition.ToPoint(), cellsNumber);
            foreach (var radius in radiusResult)
            {
                var isWalkable = false;
                while (!isWalkable)
                {
                    isWalkable = IsLocationWalkable(map, radius.ToPointPosition());
                    if (isWalkable)
                    {
                        positions.Add(radius.ToPointPosition());
                    }
                }

            }
            return Task.FromResult(positions);
        }

        public bool IsLocationWalkable(string mapId, PointPosition position)
        {

            return IsLocationWalkable(GetMap(mapId), position);

        }

        public bool IsLocationWalkable(Map map, PointPosition position)
        {
            var terrainData = map.GetTerrainAt(position.X, position.Y);

            return terrainData!.IsTransparent;
        }


        private async ValueTask GenerateMapsAsync()
        {
            var mapsToGenerate = Enumerable.Range(1, _engineConfig.Maps.Cities.Num)
                .Select(_ => Task.Run(async () =>
                {
                    var (id, map) = await BuildMapAsync(MapType.City);
                    HandleMapEvents(id, map);
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
                await Engine.DatabaseService.InsertAsync(new MapEntity()
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
                mapEntity.Layers.Add(new()
                {
                    Type = (MapLayer)gameObject.Layer,
                    Tile = baseGameObject!.Tile,
                    ObjectId = baseGameObject.ObjectId,
                    Position = new(gameObject.Position.X, gameObject.Position.Y)
                });
            }


            await BinarySerialization.SerializeToFileAsync(mapEntity,
                Path.Join(_directoriesConfig[DirectoryNameType.Maps], $"{mapId}.map"));

        }

        private async Task<(string, Map)> BuildMapAsync(MapType mapType)
        {
            var id = GenerateMapId();
            Logger.LogDebug("Generating map type {MapType}", mapType);
            return mapType switch
            {
                MapType.City => (id, await GenerateCityMapAsync()),
                MapType.Dungeon => (id, await GenerateDungeonMapAsync()),
                _ => throw new Exception($"Can't find map type generator {mapType}")
            };
        }

        private ValueTask<Map> GenerateCityMapAsync()
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

        public PointPosition GetRandomWalkablePosition(string mapId)
        {
            var map = _maps[mapId].Item1;
            var randomPosition = RandPointUtils.RandomPoint(map.Width, map.Height);
            while (map.GetTerrainAt(randomPosition) is not IGameObject terrainGameObject ||
                   !terrainGameObject.IsWalkable)
            {
                randomPosition = RandPointUtils.RandomPoint(map.Width, map.Height);
            }
            return randomPosition.ToPointPosition();
        }

        public Task<List<PointPosition>> GetFovAsync(string mapId, PointPosition sourcePosition, int radius = 5)
        {
            var map = GetMap(mapId);
            map.PlayerFOV.Reset();
            map.PlayerFOV.Calculate(sourcePosition.ToPoint(), radius);
            return Task.FromResult(map.PlayerFOV.CurrentFOV.Select(s => s.ToPointPosition()).ToList());
        }

        public async Task<bool> MovePlayerAsync(string mapId, Guid playerId, PointPosition position)
        {
            var player = await GetPlayerOnMapAsync(mapId, playerId);

            player.Position = position.ToPoint();

            return true;
        }

        private async Task<PlayerGameObject?> GetPlayerOnMapAsync(string mapId, Guid playerId)
        {
            var map = GetMap(mapId);
            return map.Entities.GetLayer((int)MapLayer.Players).Items.Cast<PlayerGameObject>()
                .FirstOrDefault(s => s.ObjectId == playerId);
        }

        public bool AddPlayerOnMap(string mapId, Guid playerId, Guid networkSessionId, PointPosition position, TileType tile)
        {
            var map = _maps[mapId].Item1;

            map.AddEntity(new PlayerGameObject(position.ToPoint())
            {
                Tile = tile,
                ObjectId = playerId,
                NetworkSessionId = networkSessionId
            });
            return true;
        }

        public bool RemovePlayerFromMap(string mapId, Guid playerId)
        {
            var map = GetMap(mapId);
            var player = map.Entities.Items.FirstOrDefault(x => x is PlayerGameObject playerGameObject && playerGameObject.ObjectId == playerId);
            if (player is null)
            {
                return false;
            }
            map.RemoveEntity(player);
            return true;
        }

        public Map GetMap(string mapId)
        {
            if (_maps.TryGetValue(mapId, out var map))
            {
                return map.Item1;
            }
            throw new Exception($"Map {mapId} not found");
        }

        public void AddEntity<TEntity>(string mapId, TEntity entity) where TEntity : IGameObject
        {
            var map = GetMap(mapId);
            map.AddEntity(entity);
        }

        public void RemoveEntity<TEntity>(string mapId, TEntity entity) where TEntity : IGameObject
        {
            var map = GetMap(mapId);
            map.RemoveEntity(entity);
        }

        public ValueTask<TEntity?> GetEntityByIdAsync<TEntity>(string mapId, Guid id) where TEntity : BaseGameObject
        {
            var map = GetMap(mapId);
            var entity = map.Entities.Items.FirstOrDefault(x => x is TEntity tEntity && tEntity.ObjectId == id);

            return ValueTask.FromResult(entity as TEntity);
        }

        public ValueTask<(string mapId, PointPosition position)> GetRandomCityStartingPointAsync()
        {
            var map = _maps.Where(x => x.Value.Item2 == MapType.City).ToList().RandomItem();
            var position = GetRandomWalkablePosition(map.Key);

            return ValueTask.FromResult((map.Key, position));

        }

        public List<PlayerGameObject> GetPlayers(string mapId)
        {
            var map = GetMap(mapId);
            return map.Entities.Items.Where(x => x is PlayerGameObject).Cast<PlayerGameObject>().ToList();
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
            Engine.EventBus.PublishAsync(new GameObjectMovedEvent(mapId, (MapLayer)gameObject.Layer, oldPosition, newPosition, baseGameObject!.ObjectId));
        }

        private void HandleGameObjectRemoved(string mapId, IGameObject gameObject, PointPosition position)
        {
            var baseGameObject = gameObject as BaseGameObject;
            Engine.EventBus.PublishAsync(new GameObjectRemovedEvent(mapId, (MapLayer)gameObject.Layer, position, baseGameObject!.ObjectId));
        }

        private ValueTask<Map> GenerateDungeonMapAsync()
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

        private static string GenerateMapId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
