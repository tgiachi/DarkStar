using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.World.Types.Equippable;
using DarkSun.Api.World.Types.Items;
using DarkSun.Database.Entities.Base;
using FreeSql.DataAnnotations;

namespace DarkSun.Database.Entities.Item
{
    [Table(Name = "items")]
    public class ItemEntity : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Weight { get; set; }
        public ItemCategoryType Category { get; set; }
        public ItemType Type { get; set; }
        public EquipLocationType EquipLocation { get; set; }
        public ItemRarityType ItemRarity { get; set; }
        public string SellDice { get; set; } = null!;
        public string BuyDice { get; set; } = null!;
        public string Attack { get; set; } = null!;
        public string Defense { get; set; } = null!;
        public string Speed { get; set; } = null!;
        public int MinLevel { get; set; }
    }
}
