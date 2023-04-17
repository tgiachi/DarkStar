using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Engine.Data.Sessions;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Database.Entities.Base;
using DarkStar.Database.Entities.Player;
using DarkStar.Database.Entities.Races;
using DarkStar.Engine.Services.Base;

using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Services
{
    [DarkStarEngineService("PlayerService", 6)]
    public class PlayerService : BaseService<PlayerService>, IPlayerService
    {
        private readonly Dictionary<Guid, PlayerSession> _playerSessions = new();

        public PlayerService(ILogger<PlayerService> logger) : base(logger)
        {
        }

        public void AddSession(Guid networkSessionId)
        {
            _playerSessions.Add(networkSessionId, new PlayerSession() { SessionId = networkSessionId });
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

        public async Task<PlayerEntity> CreatePlayerAsync(Guid accountId, string name, TileType tileId, Guid raceId,
            BaseStatEntity stats)
        {
            var race = await Engine.DatabaseService.QueryAsSingleAsync<RaceEntity>(entity => entity.Id == raceId);

            var startingPoint = await Engine.WorldService.GetRandomCityStartingPointAsync();

            var playerEntity = new PlayerEntity() { AccountId = accountId, RaceId = race.Id, Gold = 100, Name = name, MapId = startingPoint.mapId, X = startingPoint.position.X, Y = startingPoint.position.Y };
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

            await Engine.DatabaseService.InsertAsync(statEntity);

            playerEntity.Stats = statEntity;

            Logger.LogInformation("Created player {Name} for account {AccountId}", name, accountId);

            return playerEntity;
        }
    }
}
