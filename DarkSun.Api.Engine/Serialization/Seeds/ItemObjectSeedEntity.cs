using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Attributes.Seed;
using DarkSun.Api.Engine.Serialization.Seeds.Converters;
using DarkSun.Api.World.Types.Equippable;
using DarkSun.Api.World.Types.Items;
using DarkSun.Api.World.Types.Tiles;
using TinyCsv.Attributes;

namespace DarkSun.Api.Engine.Serialization.Seeds
{
    [SeedObject("Items")]
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

        [Column(converter:typeof(TileTypeConverter))]
        public TileType TileType { get; set; }

        [Column(converter:typeof(ItemCategoryTypeConverter))]
        public ItemCategoryType Category { get; set; }
        [Column(converter:typeof(ItemTypeConverter))]
        public ItemType Type { get; set; }
        [Column(converter:typeof(EquipLocationTypeConverter))]
        public EquipLocationType EquipLocation { get; set; }
        [Column(converter:typeof(ItemRarityConverter))]
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
