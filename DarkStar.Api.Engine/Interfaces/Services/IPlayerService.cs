using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Data.Player;
using DarkStar.Api.Engine.Data.Sessions;
using DarkStar.Api.Engine.Interfaces.Services.Base;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Database.Entities.Base;
using DarkStar.Database.Entities.Item;
using DarkStar.Database.Entities.Player;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Messages.World;

namespace DarkStar.Api.Engine.Interfaces.Services;

public interface IPlayerService : IDarkSunEngineService
{
    PlayerInitialInventory InitialInventory { get; }
    void AddSession(string networkSessionId);
    void RemoveSession(string networkSessionId);
    PlayerSession GetSession(string networkSessionId);
    Task<List<PlayerEntity>> GetPlayersByAccountIdAsync(Guid accountId);
    Task<PlayerEntity> CreatePlayerAsync(Guid accountId, string name, uint tileId, Guid raceId, BaseStatEntity stats);
    Task<int> AddGoldToPlayerAsync(Guid playerId, int amount);
    Task<List<PlayerInventoryEntity>> GetPlayerInventoryAsync(Guid playerId);
    Task<List<PlayerInventoryEntity>> AddPlayerInventoryAsync(Guid playerId, ItemEntity item, int amount);
    Task<List<PlayerInventoryEntity>> AddPlayerInventoryAsync(Guid playerId, Guid itemId, int amount);
    Task<bool> UpdatePlayerPositionAsync(Guid playerId, string mapId, PointPosition position);

    Task<bool> BroadcastChatMessageAsync(
        string mapId, PointPosition position, string name, uint sender, string message, WorldMessageType type
    );
}
