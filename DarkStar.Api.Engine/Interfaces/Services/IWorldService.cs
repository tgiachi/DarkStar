using DarkStar.Api.Engine.Interfaces.Services.Base;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.Engine.Map.Entities.Base;
using DarkStar.Api.World.Types.Map;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Network.Protocol.Messages.Common;
using GoRogue.GameFramework;

namespace DarkStar.Api.Engine.Interfaces.Services;

public interface IWorldService : IDarkSunEngineService
{
    PointPosition GetRandomWalkablePosition(string mapId);

    bool AddPlayerOnMap(string mapId, Guid playerId, string networkSessionId, PointPosition position, uint tile);
    bool RemovePlayerFromMap(string mapId, Guid playerId);
    GoRogue.GameFramework.Map GetMap(string mapId);
    MapType GetMapType(string mapId);
    string GetMapName(string mapId);

    void AddEntity<TEntity>(string mapId, TEntity entity) where TEntity : IGameObject;
    void RemoveEntity<TEntity>(string mapId, TEntity entity) where TEntity : IGameObject;
    ValueTask<TEntity?> GetEntityByIdAsync<TEntity>(string mapId, Guid id) where TEntity : BaseGameObject;
    ValueTask<TEntity?> GetEntityBySerialIdAsync<TEntity>(string mapId, uint serialId) where TEntity : BaseGameObject;
    ValueTask<TEntity> GetEntityByPositionAsync<TEntity>(string mapId, PointPosition position) where TEntity : BaseGameObject;
    ValueTask<List<TEntity>> GetAllEntitiesInLayerAsync<TEntity>(string mapId, MapLayer layer) where TEntity : BaseGameObject;
    ValueTask<(string mapId, PointPosition position)> GetRandomCityStartingPointAsync();
    ValueTask<List<PlayerGameObject>> GetPlayersByMapIdAsync(string mapId);
    ValueTask RemoveEntityAsync(string mapId, uint id);

    bool IsLocationWalkable(string mapId, PointPosition position);
    bool IsLocationWalkable(GoRogue.GameFramework.Map map, PointPosition position);
    Task<Dictionary<MapLayer, List<IGameObject>>> GetGameObjectsInRangeAsync(string mapId, PointPosition position, int range = 5);

    Task<List<TEntity>> GetEntitiesInRangeAsync<TEntity>(string mapId, MapLayer layer, PointPosition position, int range = 5)
        where TEntity : BaseGameObject;
    Task<List<PointPosition>> GetNeighborCellsAsync(string mapId, PointPosition startPosition, int cellsNumber = 5);
    Task<List<PointPosition>> GetFovAsync(string mapId, PointPosition sourcePosition, int radius = 5);
    Task<bool> MovePlayerAsync(string mapId, Guid playerId, PointPosition position);
    List<PlayerGameObject> GetPlayers(string mapId);
    List<PointPosition> CalculateAStarPath(string mapId, PointPosition sourcePosition, PointPosition destinationPosition);

}
