using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.World.Types.Equippable;
using DarkSun.Api.World.Types.Items;
using TinyCsv.Attributes;

namespace DarkSun.Api.Engine.Serialization.Seeds
{
    [HasHeaderRecord(true)]
    [Delimiter(";")]
    public class ItemObjectSeedEntity
    {
        [Column]
        public string Name { get; set; } = null!;
        [Column]
        public string Description { get; set; } = null!;
        [Column] 
        public int Weight { get; set; } = 1;
        [Column]
        public ItemCategoryType Category { get; set; }
        [Column]
        public ItemType Type { get; set; }
        [Column]
        public EquipLocationType EquipLocation { get; set; }
        [Column]
        public ItemRarityType ItemRarity { get; set; }
        [Column]
        public string SellDice { get; set; } = null!;
        [Column]
        public string BuyDice { get; set; } = null!;
        [Column]
        public string Attack { get; set; } = null!;
        [Column]
        public string Defense { get; set; } = null!;
        [Column]
        public string Speed { get; set; } = null!;
        [Column]
        public int MinLevel { get; set; }
    }
}
