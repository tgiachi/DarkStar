﻿using DarkStar.Api.Engine.Interfaces.Services.Base;
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

    bool AddPlayerOnMap(string mapId, Guid playerId, Guid networkSessionId, PointPosition position, TileType tile);

    bool RemovePlayerFromMap(string mapId, Guid playerId);

    GoRogue.GameFramework.Map GetMap(string mapId);

    void AddEntity<TEntity>(string mapId, TEntity entity) where TEntity : IGameObject;

    void RemoveEntity<TEntity>(string mapId, TEntity entity) where TEntity : IGameObject;
    ValueTask<TEntity?> GetEntityByIdAsync<TEntity>(string mapId, Guid id) where TEntity : BaseGameObject;

    ValueTask<(string mapId, PointPosition position)> GetRandomCityStartingPointAsync();

    Task<Dictionary<MapLayer, List<IGameObject>>> GetGameObjectsInRangeAsync(string mapId, PointPosition position, int range = 5);

    List<PlayerGameObject> GetPlayers(string mapId);
}
