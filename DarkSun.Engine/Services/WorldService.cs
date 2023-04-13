using System.Collections.Concurrent;
using System.Diagnostics;
using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Engine.Data.Config;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Api.Engine.Map.Entities;
using DarkSun.Api.Engine.Serialization;
using DarkSun.Api.World.Types.Map;
using DarkSun.Api.World.Types.Tiles;
using DarkSun.Engine.Services.Base;
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

        private readonly ConcurrentDictionary<string, (Map, MapType)> _maps = new();

        public WorldService(ILogger<WorldService> logger, EngineConfig engineConfig) : base(logger)
        {
            _engineConfig = engineConfig;
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

            return true;
        }

        private async ValueTask GenerateMapsAsync()
        {
            var mapsToGenerate = Enumerable.Range(1, _engineConfig.Maps.Cities.Num)
                .Select(_ => Task.Run(async () =>
                {
                    var (id, map) = await BuildMapAsync(MapType.City);
                    _maps.TryAdd(id, (map, MapType.City));
                }))
                .ToList();


            mapsToGenerate.AddRange(Enumerable.Range(1, _engineConfig.Maps.Dungeons.Num)
                .Select(_ => Task.Run(async () =>
                {
                    var (id, map) = await BuildMapAsync(MapType.Dungeon);
                    HandleMapEvents(id, map);
                    _maps.TryAdd(id, (map, MapType.Dungeon));
                }))
                .ToList());

            var mapGeneratingStopwatch = new Stopwatch();
            mapGeneratingStopwatch.Start();
            await Task.WhenAll(mapsToGenerate);
            mapGeneratingStopwatch.Stop();
            Logger.LogInformation("Generated {NumMaps} maps in {Time}ms", _maps.Count, mapGeneratingStopwatch.ElapsedMilliseconds);
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
            await Task.Delay(50);
            //BinarySerialization.SerializeToFileAsync()
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
                Enum.GetValues<MapLayer>().Length, Distance.Chebyshev);

            map.ApplyTerrainOverlay(wallsFloors, (pos, val) => val
                ? new TerrainGameObject(pos) { IsWalkable = true, IsTransparent = true, Tile = TileType.Null }
                : new TerrainGameObject(pos, false, false) { Tile = TileType.Null });



            return ValueTask.FromResult(map);

        }

        private void HandleMapEvents(string id, Map map)
        {
            map.ObjectAdded += (sender, args) =>
            {
                Logger.LogDebug("Added {GameObject} to map {MapId}", args.Item, id);
            };
            map.ObjectMoved += (sender, args) =>
            {
                Logger.LogDebug("Moved {GameObject} to map {MapId}", args.Item, id);
            };
            map.ObjectRemoved += (sender, args) =>
            {
                Logger.LogDebug("Removed {GameObject} from map {MapId}", args.Item, id);
            };
        }

        private ValueTask<Map> GenerateDungeonMapAsync(string id)
        {
            var dungeonGenerator = new Generator(_engineConfig.Maps.Dungeons.Width, _engineConfig.Maps.Dungeons.Height)
                .ConfigAndGenerateSafe(generator =>
                {
                    generator.AddSteps(DefaultAlgorithms.DungeonMazeMapSteps());
                }, 3);

            var map = new Map(_engineConfig.Maps.Dungeons.Width, _engineConfig.Maps.Dungeons.Height,
                Enum.GetValues<MapLayer>().Length, Distance.Chebyshev);

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
