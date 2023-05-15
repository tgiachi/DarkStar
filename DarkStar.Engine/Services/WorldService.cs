using System.Collections.Concurrent;
using System.Diagnostics;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Data.Config;
using DarkStar.Api.Engine.Data.Blueprint;
using DarkStar.Api.Engine.Data.Config;
using DarkStar.Api.Engine.Events.Map;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.Engine.Map.Entities.Base;
using DarkStar.Api.Engine.Map.Enums;
using DarkStar.Api.Engine.Serialization;
using DarkStar.Api.Engine.Serialization.Map;
using DarkStar.Api.Engine.Utils;
using DarkStar.Api.Utils;
using DarkStar.Api.World.Types.Map;
using DarkStar.Database.Entities.Maps;
using DarkStar.Engine.Services.Base;
using DarkStar.Network.Protocol.Messages.Common;
using FastEnumUtility;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using Humanizer;
using Microsoft.Extensions.Logging;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace DarkStar.Engine.Services;

[DarkStarEngineService("WorldService", 10)]
public class WorldService : BaseService<IWorldService>, IWorldService
{
    private readonly EngineConfig _engineConfig;
    private readonly DirectoriesConfig _directoriesConfig;
    private readonly Radius _positionRadius = new();

    private readonly ConcurrentDictionary<string, (Map map, MapType mapType, MapInfo mapInfo)> _maps = new();

    public WorldService(
        ILogger<WorldService> logger, EngineConfig engineConfig, DirectoriesConfig directoriesConfig
    ) : base(logger)
    {
        _engineConfig = engineConfig;
        _directoriesConfig = directoriesConfig;
    }

    public override async ValueTask<bool> StopAsync()
    {
        await SaveMapsAsync();
        return true;
    }

    protected override async ValueTask<bool> StartAsync()
    {
        if (_engineConfig.Database.RecreateDatabase)
        {
            await GenerateMapsAsync();
        }
        else
        {
            await LoadMapsAsync();
        }

        await SaveMapsAsync();
        Engine.JobSchedulerService.AddJob(
            "SaveMaps",
            () => { _ = Task.Run(SaveMapsAsync); },
            (int)TimeSpan.FromMinutes(_engineConfig.Maps.SaveEveryMinutes).TotalSeconds,
            false
        );

        return true;
    }

    private async Task LoadMapsAsync()
    {
        var sw = new Stopwatch();
        sw.Start();
        var maps = await Engine.DatabaseService.FindAllAsync<MapEntity>();
        Logger.LogInformation("Loading maps {MapsCount}", maps.Count);
        if (maps.Count == 0)
        {
            await GenerateMapsAsync();
            return;
        }

        foreach (var map in maps)
        {
            await LoadMap(map);
        }

        sw.Stop();
        Logger.LogInformation("Loaded maps in {Elapsed}", sw.Elapsed.TotalMilliseconds.Milliseconds());
    }

    private async Task LoadMap(MapEntity entity)
    {
        Logger.LogInformation("Loading map {Type} {MapId}", entity.Type, entity.Id);
        var mapData = await BinarySerialization.DeserializeFromFileAsync<MapObjectSerialization>(
            Path.Join(_directoriesConfig[DirectoryNameType.Maps], entity.FileName)
        );

        var map = new Map(mapData.Width, mapData.Height, FastEnum.GetValues<MapLayer>().Count, Distance.Chebyshev);
        _maps.TryAdd(mapData.MapId, (map, mapData.MapType, new MapInfo { Name = mapData.Name }));

        HandleMapEvents(mapData.MapId, map);
        foreach (var layer in mapData.Layers)
        {
            if (layer.Type == MapLayer.Terrain)
            {
                var terrain = new TerrainGameObject(layer.Position.ToPoint())
                {
                    Tile = layer.Tile,
                    ObjectId = layer.ObjectId
                };
                map.SetTerrain(terrain);
            }

            if (layer.Type == MapLayer.Creatures)
            {
                var npc = new NpcGameObject(layer.Position.ToPoint())
                {
                    Tile = layer.Tile,
                    ObjectId = layer.ObjectId,
                    Name = ""
                };
                map.AddEntity(npc);
            }

            if (layer.Type == MapLayer.Objects)
            {
                var gameObject = new WorldGameObject(layer.Position.ToPoint())
                {
                    ObjectId = layer.ObjectId,
                    Tile = layer.Tile
                };
                map.AddEntity(gameObject);
            }
        }
    }

    public Task<Dictionary<MapLayer, List<IGameObject>>> GetGameObjectsInRangeAsync(
        string mapId,
        PointPosition position, int range = 5
    )
    {
        var map = GetMap(mapId);
        var gameObjects = FastEnum.GetValues<MapLayer>().ToDictionary(layer => layer, _ => new List<IGameObject>());

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

    public Task<List<TEntity>> GetEntitiesInRangeAsync<TEntity>(
        string mapId, MapLayer layer, PointPosition position, int range = 5
    )
        where TEntity : BaseGameObject
    {
        var entities = new List<TEntity>();
        var map = GetMap(mapId);
        var rangePositions = _positionRadius.PositionsInRadius(position.ToPoint(), range);
        foreach (var pos in rangePositions)
        {
            var foundObjects = map.Entities.GetItemsAt(pos);
            foreach (var gameObject in foundObjects)
            {
                if (gameObject is TEntity entity)
                {
                    entities.Add(entity);
                }
            }
        }

        return Task.FromResult(entities);
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

    public ValueTask<List<PlayerGameObject>> GetPlayersByMapIdAsync(string mapId)
    {
        var map = GetMap(mapId);
        return ValueTask.FromResult(
            map.Entities.Items.Where(s => s.Layer == (int)MapLayer.Players).Cast<PlayerGameObject>().ToList()
        );
    }

    public async ValueTask RemoveEntityAsync(string mapId, uint id)
    {
        var map = GetMap(mapId);
        var entity = await GetEntityBySerialIdAsync<BaseGameObject>(mapId, id);
        map.RemoveEntity(entity);
    }

    public bool IsLocationWalkable(string mapId, PointPosition position) => IsLocationWalkable(GetMap(mapId), position);

    public bool IsLocationWalkable(Map map, PointPosition position)
    {
        var terrainData = map.GetTerrainAt(position.X, position.Y);

        return terrainData!.IsTransparent;
    }


    private async ValueTask GenerateMapsAsync()
    {
        var mapsToGenerate = Enumerable.Range(1, _engineConfig.Maps.Cities.Num)
            .Select(
                _ => Task.Run(
                    async () =>
                    {
                        var (id, map) = await BuildMapAsync(MapType.City);
                        HandleMapEvents(id, map);
                        _maps.TryAdd(id, (map, MapType.City, new MapInfo()));
                    }
                )
            )
            .ToList();


        mapsToGenerate.AddRange(
            Enumerable.Range(1, _engineConfig.Maps.Dungeons.Num)
                .Select(
                    _ => Task.Run(
                        async () =>
                        {
                            var (id, map) = await BuildMapAsync(MapType.Dungeon);
                            HandleMapEvents(id, map);
                            _maps.TryAdd(id, (map, MapType.Dungeon, new MapInfo()));
                        }
                    )
                )
                .ToList()
        );

        var mapGeneratingStopwatch = new Stopwatch();
        mapGeneratingStopwatch.Start();
        await Task.WhenAll(mapsToGenerate);
        await Task.WhenAll(FillMaps());
        await SaveMapsOnDbAsync();
        mapGeneratingStopwatch.Stop();

        Logger.LogInformation(
            "Generated {NumMaps} maps in {Time}ms",
            _maps.Count,
            mapGeneratingStopwatch.ElapsedMilliseconds
        );
    }

    private async ValueTask SaveMapsOnDbAsync()
    {
        foreach (var maps in _maps)
        {
            var map = maps.Value;
            var mapId = maps.Key;
            await Engine.DatabaseService.InsertAsync(
                new MapEntity
                {
                    Name = map.Item3.Name,
                    MapId = mapId,
                    Type = map.Item2,
                    FileName = $"{mapId}.map"
                }
            );
        }
    }

    private List<Task> FillMaps()
    {
        var tasksToExecute = new List<Task>();

        tasksToExecute.AddRange(
            _maps.Where(s => s.Value.mapType == MapType.City)
                .Select(k => FillCityMapAsync(k.Key))
        );

        return tasksToExecute;
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
        var mapEntity = new MapObjectSerialization
        {
            Name = map.Item3.Name, MapId = mapId, MapType = map.Item2, Height = map.Item1.Height, Width = map.Item1.Width
        };

        foreach (var terrainPosition in map.Item1.Terrain.Positions())
        {
            var terrainObject = map.Item1.GetTerrainAt(terrainPosition) as BaseGameObject;
            mapEntity.Layers.Add(
                new LayerObjectSerialization
                {
                    Type = MapLayer.Terrain,
                    Tile = terrainObject!.Tile,
                    Position = new PointPosition(terrainPosition.X, terrainPosition.Y)
                }
            );
        }

        foreach (var gameObject in map.Item1.Entities.Items)
        {
            var baseGameObject = gameObject as BaseGameObject;
            mapEntity.Layers.Add(
                new LayerObjectSerialization
                {
                    Type = (MapLayer)gameObject.Layer,
                    Tile = baseGameObject!.Tile,
                    ObjectId = baseGameObject.ObjectId,
                    Position = gameObject.Position.ToPointPosition()
                }
            );
        }

        await BinarySerialization.SerializeToFileAsync(
            mapEntity,
            Path.Join(_directoriesConfig[DirectoryNameType.Maps], $"{mapId}.map")
        );
    }

    private async Task<(string, Map)> BuildMapAsync(MapType mapType)
    {
        var id = GenerateMapId();
        Logger.LogDebug("Generating map type {MapType}", mapType);
        return mapType switch
        {
            MapType.City    => (id, await GenerateCityMapAsync()),
            MapType.Dungeon => (id, await GenerateDungeonMapAsync()),
            _               => throw new Exception($"Can't find map type generator {mapType}")
        };
    }

    private (BlueprintMapInfoContext context, Generator generator, ArrayView<bool> terrainFloor, Map map) BuildMapGenerator(
        MapType mapType
    )
    {
        var mapGenerator = Engine.BlueprintService.GetMapGenerator(mapType);
        var genericMapGenerator = new Generator(mapGenerator.Width, mapGenerator.Height)
            .ConfigAndGenerateSafe(
                generator =>
                {
                    switch (((MapGeneratorType)mapGenerator.MapStrategy))
                    {
                        case MapGeneratorType.RectangleMap:
                            generator.AddSteps(DefaultAlgorithms.RectangleMapSteps());
                            break;
                        case MapGeneratorType.Dungeon:
                            generator.AddSteps(DefaultAlgorithms.DungeonMazeMapSteps());
                            break;
                        case MapGeneratorType.BasicRandomRooms:
                            generator.AddSteps(DefaultAlgorithms.BasicRandomRoomsMapSteps());
                            break;
                        case MapGeneratorType.CellularAutomata:
                            generator.AddSteps(DefaultAlgorithms.CellularAutomataGenerationSteps());
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                },
                3
            );

        var wallsFloors = genericMapGenerator.Context.GetFirst<ArrayView<bool>>("WallFloor");
        var map = new Map(
            mapGenerator.Width,
            mapGenerator.Height,
            FastEnum.GetValues<MapLayer>().Count,
            Distance.Chebyshev
        );

        return (mapGenerator, genericMapGenerator, wallsFloors, map);
    }


    private ValueTask<Map> GenerateCityMapAsync()
    {
        var generator = BuildMapGenerator(MapType.City);


        generator.map.ApplyTerrainOverlay(
            generator.terrainFloor,
            (pos, val) => val
                ? new TerrainGameObject(pos)
                    { IsWalkable = true, IsTransparent = true, Tile = generator.context.NonBlockingTile.Id }
                : new TerrainGameObject(pos, false, false) { Tile = generator.context.BlockingTile.Id }
        );

        return ValueTask.FromResult(generator.map);
    }

    private ValueTask<Map> GenerateDungeonMapAsync()
    {
        var generator = BuildMapGenerator(MapType.Dungeon);


        generator.map.ApplyTerrainOverlay(
            generator.terrainFloor,
            (pos, val) => val
                ? new TerrainGameObject(pos)
                    { IsWalkable = true, IsTransparent = true, Tile = generator.context.NonBlockingTile.Id }
                : new TerrainGameObject(pos, false, false) { Tile = generator.context.BlockingTile.Id }
        );

        return ValueTask.FromResult(generator.map);
    }

    private async Task FillCityMapAsync(string mapId)
    {
        var context = Engine.BlueprintService.GetMapFiller(mapId, MapType.City);
        foreach (var worldGameObject in context.GameObjects)
        {
            AddEntity(mapId, worldGameObject);
        }

        foreach (var npcGameObject in context.Npcs)
        {
            AddEntity(mapId, npcGameObject);
        }
    }

    private void HandleMapEvents(string id, Map map)
    {
        map.ObjectAdded += (_, args) =>
        {
            if (args.Item is not TerrainGameObject)
            {
                Logger.LogDebug(
                    "Added {GameObject} to map {MapId} Layer: {Layer}",
                    args.Item,
                    id,
                    ((MapLayer)args.Item.Layer).FastToString()
                );
                HandleGameObjectAdded(id, args.Item, args.Position.ToPointPosition());
            }
        };
        map.ObjectMoved += (_, args) =>
        {
            if (args.Item is not TerrainGameObject)
            {
                Logger.LogDebug(
                    "Moved {GameObject} to map {MapId} Layer: {Layer}",
                    args.Item,
                    id,
                    ((MapLayer)args.Item.Layer).FastToString()
                );
                HandleGameObjectMoved(id, args.Item, args.OldPosition.ToPointPosition(), args.NewPosition.ToPointPosition());
            }
        };
        map.ObjectRemoved += (_, args) =>
        {
            if (args.Item is not TerrainGameObject)
            {
                Logger.LogDebug(
                    "Removed {GameObject} to map {MapId} Layer: {Layer}",
                    args.Item,
                    id,
                    ((MapLayer)args.Item.Layer).FastToString()
                );
                HandleGameObjectRemoved(id, args.Item, args.Position.ToPointPosition());
            }
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

    public Task<List<VisibilityPointPosition>> GetFovAsync(string mapId, PointPosition sourcePosition, int radius = 15)
    {
        var map = GetMap(mapId);
        map.PlayerFOV.Reset();
        map.PlayerFOV.Calculate(sourcePosition.ToPoint(), radius);
        var s = map.PlayerFOV.DoubleResultView;
        return Task.FromResult(
            map.PlayerFOV.CurrentFOV.Select(s => new VisibilityPointPosition(s.X, s.Y, map.PlayerFOV.DoubleResultView[s]))
                .ToList()
        );
    }

    public async Task<bool> MovePlayerAsync(string mapId, Guid playerId, PointPosition position)
    {
        var player = await GetPlayerOnMapAsync(mapId, playerId);
        if (player is null)
        {
            return false;
        }

        player.Position = position.ToPoint();

        return true;
    }

    private async Task<PlayerGameObject?> GetPlayerOnMapAsync(string mapId, Guid playerId)
    {
        var map = GetMap(mapId);
        return map.Entities.GetLayer((int)MapLayer.Players)
            .Items.Cast<PlayerGameObject>()
            .FirstOrDefault(s => s.ObjectId == playerId);
    }

    public bool AddPlayerOnMap(string mapId, Guid playerId, string networkSessionId, PointPosition position, uint tile)
    {
        var map = _maps[mapId].Item1;

        map.AddEntity(
            new PlayerGameObject(position.ToPoint())
            {
                Tile = tile,
                ObjectId = playerId,
                NetworkSessionId = networkSessionId
            }
        );
        return true;
    }

    public bool RemovePlayerFromMap(string mapId, Guid playerId)
    {
        var map = GetMap(mapId);
        var player = map.Entities.Items.FirstOrDefault(
            x => x is PlayerGameObject playerGameObject && playerGameObject.ObjectId == playerId
        );
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

    public MapType GetMapType(string mapId)
    {
        _maps.TryGetValue(mapId, out var map);

        return map.mapType;
    }

    public string GetMapName(string mapId)
    {
        _maps.TryGetValue(mapId, out var map);

        return map.mapInfo.Name;
    }

    public void AddEntity<TEntity>(string mapId, TEntity entity) where TEntity : IGameObject
    {
        var map = GetMap(mapId);
        Logger.LogDebug(
            "Add entity {Entity} to map {MapId} Layer: {Layer} Position: {Pos}",
            entity,
            mapId,
            ((MapLayer)entity.Layer).FastToString(),
            entity.Position
        );
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

    public ValueTask<TEntity?> GetEntityBySerialIdAsync<TEntity>(string mapId, uint serialId) where TEntity : BaseGameObject
    {
        var map = GetMap(mapId);
        var entity = map.Entities.Items.FirstOrDefault(x => x is TEntity tEntity && tEntity.ID == serialId);
        return ValueTask.FromResult(entity as TEntity);
    }

    public ValueTask<TEntity> GetEntityByPositionAsync<TEntity>(string mapId, PointPosition position)
        where TEntity : BaseGameObject
    {
        var map = GetMap(mapId);
        var entity = map.GetEntityAt<TEntity>(position.ToPoint());
        return ValueTask.FromResult(entity);
    }

    public ValueTask<List<TEntity>> GetAllEntitiesInLayerAsync<TEntity>(string mapId, MapLayer layer)
        where TEntity : BaseGameObject
    {
        var map = GetMap(mapId);
        if (layer == MapLayer.Terrain)
        {
            return ValueTask.FromResult(map.Terrain.Positions().Select(x => map.GetTerrainAt(x)).Cast<TEntity>().ToList());
        }

        var entities = map.Entities.GetLayer((int)layer).Items.Cast<TEntity>().ToList();
        return ValueTask.FromResult(entities);
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


    public List<PointPosition> CalculateAStarPath(
        string mapId, PointPosition sourcePosition, PointPosition destinationPosition
    )
    {
        var map = GetMap(mapId);
        var path = map.AStar.ShortestPath(sourcePosition.ToPoint(), destinationPosition.ToPoint());

        return path.StepsWithStart.Select(s => s.ToPointPosition()).ToList();
    }

    private void HandleGameObjectAdded(string mapId, IGameObject gameObject, PointPosition position)
    {
        var baseGameObject = gameObject as BaseGameObject;
        Engine.EventBus.PublishAsync(
            new GameObjectAddedEvent(
                mapId,
                (MapLayer)gameObject.Layer,
                position,
                baseGameObject!.ObjectId,
                gameObject.ID
            )
        );
    }

    private void HandleGameObjectMoved(
        string mapId, IGameObject gameObject, PointPosition oldPosition, PointPosition newPosition
    )
    {
        var baseGameObject = gameObject as BaseGameObject;
        Engine.EventBus.PublishAsync(
            new GameObjectMovedEvent(
                mapId,
                (MapLayer)gameObject.Layer,
                oldPosition,
                newPosition,
                baseGameObject!.ObjectId,
                baseGameObject.ID
            )
        );
    }

    private void HandleGameObjectRemoved(string mapId, IGameObject gameObject, PointPosition position)
    {
        var baseGameObject = gameObject as BaseGameObject;
        Engine.EventBus.PublishAsync(
            new GameObjectRemovedEvent(
                mapId,
                (MapLayer)gameObject.Layer,
                position,
                baseGameObject!.ObjectId,
                baseGameObject.ID
            )
        );
    }


    private static string GenerateMapId() => Guid.NewGuid().ToString().Replace("-", "");
}
