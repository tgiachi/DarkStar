﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Database.Entities.Races;
using DarkStar.Network.Protocol.Messages.Players;

namespace DarkStar.Engine.MessageListeners.Helpers
{
    public class PlayerDataHelper
    {
        public static async Task<PlayerListResponseMessage> BuildPlayerListForPlayerAsync(IDarkSunEngine engine,
            Guid accountId)
        {
            var playerList = await engine.PlayerService.GetPlayersByAccountIdAsync(accountId);
            return new PlayerListResponseMessage()
            {
                Players = playerList.Select(p => new PlayerObjectMessage()
                {
                    Id = p.Id,
                    Level = p.Stats.Level,
                    Name = p.Name,
                    Tile = p.TileId,
                    Race = ""
                }).ToList()
            };
        }

        public static async Task<PlayerRacesResponseMessage> BuildPlayerRacesAsync(IDarkSunEngine engine)
        {
            var races = await engine.DatabaseService.QueryAsListAsync<RaceEntity>(entity => entity.IsVisible);

            return new PlayerRacesResponseMessage()
            {
                Races = races.Select(r => new PlayerRaceObject()
                {
                    RaceId = r.Id,
                    Dexterity = r.Dexterity,
                    Intelligence = r.Intelligence,
                    Strength = r.Strength,
                    Name = r.Name,
                    Luck = r.Luck,
                    TileId = r.TileId
                }).ToList()
            };
        }
    }
}