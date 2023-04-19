using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Api.Engine.Data.Player
{
    public class PlayerInitialInventoryItem
    {
        public Guid ItemId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
