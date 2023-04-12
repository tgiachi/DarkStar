using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Engine.Data.Sessions;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Api.World.Types.Tiles;
using DarkSun.Database.Entities.Base;
using DarkSun.Database.Entities.Player;
using DarkSun.Database.Entities.Races;
using DarkSun.Engine.Services.Base;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.Services;

[DarkSunEngineService("PlayerService", 6)]
public class PlayerService : BaseService<PlayerService>, IPlayerService
{
    private readonly Dictionary<Guid, PlayerSession> _playerSessions = new();

    public PlayerService(ILogger<PlayerService> logger) : base(logger)
    {
    }

    public void AddSession(Guid networkSessionId)
    {
        _playerSessions.Add(networkSessionId, new PlayerSession());
    }

    public void RemoveSession(Guid networkSessionId)
    {
        _playerSessions.Remove(networkSessionId);
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

    public async Task<PlayerEntity> CreatePlayerAsync(Guid accountId, TileType tileId, Guid raceId, BaseStatEntity stats)
    {
        var race = await Engine.DatabaseService.QueryAsSingleAsync<RaceEntity>(entity => entity.Id == raceId);
        var playerEntity = new PlayerEntity() { AccountId = accountId, RaceId = race.Id, Gold = 100 };
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
            Experience = 0,
        };

        await Engine.DatabaseService.InsertAsync(statEntity);

        playerEntity.Stats = statEntity;

        return playerEntity;
    }
}
