using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Api.Engine.Data.Player;

public class PlayerInitialInventory
{
    public int Gold { get; set; }
    public List<PlayerInitialInventoryItem> Items { get; set; } = new();
}
