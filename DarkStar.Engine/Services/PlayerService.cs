using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Engine.Data.Player;
using DarkStar.Api.Engine.Data.Sessions;
using DarkStar.Api.Engine.Events.Players;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.Utils;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Database.Entities.Base;
using DarkStar.Database.Entities.Item;
using DarkStar.Database.Entities.Player;
using DarkStar.Database.Entities.Races;
using DarkStar.Engine.Services.Base;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Messages.World;
using Microsoft.Extensions.Logging;
using SadRogue.Primitives;

namespace DarkStar.Engine.Services;

[DarkStarEngineService("PlayerService", 6)]
public class PlayerService : BaseService<PlayerService>, IPlayerService
{
    private readonly SemaphoreSlim _playerLock = new(1);
    private readonly Dictionary<Guid, PlayerSession> _playerSessions = new();

    public PlayerInitialInventory InitialInventory { get; private set; } = new();

    public PlayerService(ILogger<PlayerService> logger) : base(logger)
    {
    }

    public void AddSession(Guid networkSessionId)
    {
        _playerLock.Wait();
        _playerSessions.Add(networkSessionId, new PlayerSession() { SessionId = networkSessionId });
        _playerLock.Release();
    }

    public void RemoveSession(Guid networkSessionId)
    {
        _playerLock.Wait();
        _playerSessions.Remove(networkSessionId);
        _playerLock.Release();
    }

    public PlayerSession GetSession(Guid networkSessionId)
    {
        if (_playerSessions.TryGetValue(networkSessionId, out var session))
        {
            return session;
        }

        throw new Exception($"Can't find network sessionId {networkSessionId}");
    }

    public async Task<List<PlayerEntity>> GetPlayersByAccountIdAsync(Guid accountId)
    {
        var players =
            await Engine.DatabaseService.QueryAsListAsync<PlayerEntity>(entity => entity.AccountId == accountId);

        foreach (var player in players)
        {
            player.Stats = await Engine.DatabaseService.QueryAsSingleAsync<PlayerStatEntity>(entity =>
                entity.PlayerId == player.Id);
        }

        return players;
    }

    public async Task<PlayerEntity> CreatePlayerAsync(Guid accountId, string name, uint tileId, Guid raceId,
        BaseStatEntity stats)
    {
        var race = await Engine.DatabaseService.QueryAsSingleAsync<RaceEntity>(entity => entity.Id == raceId);

        var startingPoint = await Engine.WorldService.GetRandomCityStartingPointAsync();

        var playerEntity = new PlayerEntity() { AccountId = accountId, TileId = tileId, RaceId = race.Id, Gold = InitialInventory.Gold, Name = name, MapId = startingPoint.mapId, X = startingPoint.position.X, Y = startingPoint.position.Y };
        await Engine.DatabaseService.InsertAsync(playerEntity);

        var statEntity = new PlayerStatEntity()
        {
            PlayerId = playerEntity.Id,
            Strength = stats.Strength,
            Dexterity = stats.Dexterity,
            Intelligence = stats.Intelligence,
            Health = 10,
            MaxHealth = 10,
            Luck = stats.Luck,
            Mana = 10,
            MaxMana = 10,
            Level = 1,
            Experience = 0
        };

        foreach (var item in InitialInventory.Items)
        {
            var inventoryEntity = new PlayerInventoryEntity() { PlayerId = playerEntity.Id, ItemId = item.ItemId, Amount = item.Quantity };
            await Engine.DatabaseService.InsertAsync(inventoryEntity);
        }


        await Engine.DatabaseService.InsertAsync(statEntity);

        playerEntity.Stats = statEntity;

        Logger.LogInformation("Created player {Name} for account {AccountId}", name, accountId);

        Engine.EventBus.PublishAsync(new PlayerCreatedEvent(playerEntity.Id, playerEntity.Name));

        return playerEntity;
    }

    public async Task<int> AddGoldToPlayerAsync(Guid playerId, int amount)
    {
        var player = await GetPlayerByIdAsync(playerId);

        player.Gold += amount;

        await Engine.DatabaseService.UpdateAsync(player);

        Engine.EventBus.PublishAsync(new PlayerGoldChangedEvent(playerId, amount, player.Gold));

        return player.Gold;
    }

    public async Task<List<PlayerInventoryEntity>> GetPlayerInventoryAsync(Guid playerId)
    {
        var inventories = await Engine.DatabaseService.QueryAsListAsync<PlayerInventoryEntity>(entity => entity.PlayerId == playerId);
        foreach (var inventory in inventories)
        {
            var item = await Engine.DatabaseService.QueryAsSingleAsync<ItemEntity>(entity => entity.Id == inventory.ItemId);
            inventory.Item = item;
        }

        return inventories;
    }

    public Task<List<PlayerInventoryEntity>> AddPlayerInventoryAsync(Guid playerId, ItemEntity item, int amount) => AddPlayerInventoryAsync(playerId, item.Id, amount);

    public async Task<List<PlayerInventoryEntity>> AddPlayerInventoryAsync(Guid playerId, Guid itemId, int amount)
    {
        var inventories = await GetPlayerInventoryAsync(playerId);

        var inventory = inventories.FirstOrDefault(entity => entity.ItemId == itemId);

        if (inventory != null)
        {
            inventory = new PlayerInventoryEntity() { Amount = amount, ItemId = itemId, PlayerId = playerId };
            await Engine.DatabaseService.InsertAsync(inventory);
        }
        else
        {
            inventory!.Amount += amount;
            await Engine.DatabaseService.UpdateAsync(inventory);
        }
        return await GetPlayerInventoryAsync(playerId);
    }

    public async Task<bool> UpdatePlayerPositionAsync(Guid playerId, string mapId, PointPosition position)
    {
        var player = await GetPlayerByIdAsync(playerId);
        player.MapId = mapId;
        player.X = position.X;
        player.Y = position.Y;

        await Engine.DatabaseService.UpdateAsync(player);
        await Engine.WorldService.MovePlayerAsync(mapId, playerId, position);
        return true;
    }

    public async Task<bool> BroadcastChatMessageAsync(
        string mapId, PointPosition position, string name, uint sender, string message, WorldMessageType type
    )
    {
        var players = await Engine.WorldService.GetPlayersByMapIdAsync(mapId);
        var radius = new Radius();
        var positions = radius.PositionsInRadius(position.ToPoint(), (int)type).ToList();
        var worldMessage = new WorldMessageResponseMessage(message, sender.ToString(), name, type);
        foreach (var player in players)
        {
            if (positions.Contains(player.Position))
            {
                await Engine.NetworkServer.SendMessageAsync(player.NetworkSessionId, worldMessage);
            }
        }

        return true;
    }

    private async Task<PlayerEntity> GetPlayerByIdAsync(Guid playerId)
    {
        var player = await Engine.DatabaseService.QueryAsSingleAsync<PlayerEntity>(entity => entity.Id == playerId);
        return player ?? throw new Exception("Player not found");
    }
}
