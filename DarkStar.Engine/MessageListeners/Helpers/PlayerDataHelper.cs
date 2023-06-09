using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Database.Entities.Item;
using DarkStar.Database.Entities.Player;
using DarkStar.Database.Entities.Races;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Messages.Players;

namespace DarkStar.Engine.MessageListeners.Helpers;

public static class PlayerDataHelper
{
    public static async Task<PlayerListResponseMessage> BuildPlayerListForPlayerAsync(
        IDarkSunEngine engine,
        Guid accountId
    )
    {
        var playerList = await engine.PlayerService.GetPlayersByAccountIdAsync(accountId);
        return new PlayerListResponseMessage()
        {
            Players = playerList.Select(
                    p => new PlayerObjectMessage()
                    {
                        Id = p.Id,
                        Level = p.Stats.Level,
                        Name = p.Name,
                        Tile = p.TileId,
                        Race = ""
                    }
                )
                .ToList()
        };
    }

    public static async Task<PlayerStatsResponseMessage> BuildPlayerStatsAsync(IDarkSunEngine engine, Guid playerId)
    {
        var stats = await engine.DatabaseService.QueryAsSingleAsync<PlayerStatEntity>(entity => entity.PlayerId == playerId);

        return new PlayerStatsResponseMessage(
            stats.Level,
            stats.Experience,
            stats.Strength,
            stats.Dexterity,
            stats.Intelligence,
            stats.Luck,
            stats.MaxHealth,
            stats.MaxMana,
            stats.Health,
            stats.Mana
        );
    }

    public static async Task<PlayerRacesResponseMessage> BuildPlayerRacesAsync(IDarkSunEngine engine)
    {
        var races = await engine.DatabaseService.QueryAsListAsync<RaceEntity>(entity => entity.IsVisible);

        return new PlayerRacesResponseMessage()
        {
            Races = races.Select(
                    r => new PlayerRaceObject()
                    {
                        RaceId = r.Id,
                        Dexterity = r.Dexterity,
                        Intelligence = r.Intelligence,
                        Strength = r.Strength,
                        Name = r.Name,
                        Luck = r.Luck,
                        TileId = (int)r.TileId
                    }
                )
                .ToList()
        };
    }

    public static async Task<PlayerInventoryResponseMessage> BuildPlayerInventoryAsync(IDarkSunEngine engine, Guid playerId)
    {
        var message = new PlayerInventoryResponseMessage();

        var inventory = await engine.PlayerService.GetPlayerInventoryAsync(playerId);
        foreach (var inv in inventory)
        {
            var item = await engine.DatabaseService.QueryAsSingleAsync<ItemEntity>(
                entity => entity.Id == inv.ItemId
            );
            message.Items.Add(
                new PlayerInventoryItem
                {
                    Quantity = inv.Amount,
                    ItemDescription = inv.Item.Description,
                    ItemName = inv.Item.Name,
                    ItemId = inv.ItemId,
                    TileId = item.TileType
                }
            );
        }

        return message;
    }

    public static async Task<PlayerDataResponseMessage> BuildPlayerDataResponse(IDarkSunEngine engine, Guid playerId)
    {
        var player = await engine.DatabaseService.QueryAsSingleAsync<PlayerEntity>(
            entity => entity.Id == playerId
        );


        return new PlayerDataResponseMessage(
            player.MapId,
            player.TileId,
            new PointPosition(player.X, player.Y),
            player.Id,
            player.Name,
            1
        );
    }
}
