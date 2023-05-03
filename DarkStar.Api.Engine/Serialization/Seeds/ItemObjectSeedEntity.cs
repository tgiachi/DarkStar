using DarkStar.Api.Attributes.Seed;
using DarkStar.Api.Engine.Serialization.Seeds.Converters;
using DarkStar.Api.World.Types.Equippable;
using DarkStar.Api.World.Types.Items;
using TinyCsv.Attributes;

namespace DarkStar.Api.Engine.Serialization.Seeds;

[SeedObject("Items")]
[HasHeaderRecord(true)]
[Delimiter(";")]
public class ItemObjectSeedEntity
{
    [Column] public string Name { get; set; } = null!;
    [Column] public string Description { get; set; } = null!;
    [Column] public int Weight { get; set; } = 1;
    [Column] public string TileName { get; set; }
    [Column] public string Category { get; set; }
    [Column] public string Type { get; set; }

    [Column(converter: typeof(EquipLocationTypeConverter))]
    public EquipLocationType EquipLocation { get; set; }

    [Column(converter: typeof(ItemRarityConverter))]
    public ItemRarityType ItemRarity { get; set; }

    [Column] public string SellDice { get; set; } = null!;
    [Column] public string BuyDice { get; set; } = null!;
    [Column] public string Attack { get; set; } = null!;
    [Column] public string Defense { get; set; } = null!;
    [Column] public string Speed { get; set; } = null!;
    [Column] public int MinLevel { get; set; }
}
