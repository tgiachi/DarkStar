using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Data.Player;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.ScriptModules;
using DarkStar.Api.Utils;
using DarkStar.Database.Entities.Item;
using DarkStar.Engine.Attributes.ScriptEngine;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.ScriptModules;

[ScriptModule]
public class PlayerScriptModule : BaseScriptModule
{
    public PlayerScriptModule(ILogger<BaseScriptModule> logger, IDarkSunEngine engine) : base(logger, engine)
    {
    }

    [ScriptFunction("set_player_initial_gold")]
    public void SetInitialGold(int gold)
    {
        Engine.PlayerService.InitialInventory.Gold = gold;
        Logger.LogInformation("Players Initial gold set: {Gold}", gold);
    }

    [ScriptFunction("set_player_initial_item")]
    public void SetInitialItem(string itemName, int quantity)
    {
        _ = Task.Run(
            async () =>
            {
                var item = await Engine.DatabaseService.QueryAsSingleAsync<ItemEntity>(
                    entity => SearchListUtils.MatchesWildcard(entity.Name, itemName)
                );
                if (item == null)
                {
                    Logger.LogWarning("Item {ItemName} not found", itemName);
                    return;
                }

                Engine.PlayerService.InitialInventory.Items.Add(
                    new PlayerInitialInventoryItem()
                    {
                        ItemId = item.Id,
                        Quantity = quantity
                    }
                );
                Logger.LogInformation("Players Initial item set: {ItemId} x {Quantity}", item.Name, quantity);
            }
        );
    }
}
