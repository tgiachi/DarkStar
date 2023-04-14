using DarkSun.Api.Engine.Interfaces.Services.Base;
using DarkSun.Api.World.Types.Tiles;
using DarkSun.Network.Protocol.Messages.Common;
using GoRogue.GameFramework;

namespace DarkSun.Api.Engine.Interfaces.Services;

public interface IWorldService : IDarkSunEngineService
{
    PointPosition GetRandomWalkablePosition(string mapId);

    bool AddPlayerOnMap(string mapId, Guid playerId, PointPosition position, TileType tile);

    bool RemovePlayerFromMap(string mapId, Guid playerId);

    GoRogue.GameFramework.Map GetMap(string mapId);

    void AddEntity<TEntity>(string mapId, TEntity entity) where TEntity : IGameObject;

}
