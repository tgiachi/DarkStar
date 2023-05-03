using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Attributes.Seed;
using DarkStar.Api.Serialization.Converters;
using DarkStar.Api.World.Types.Tiles;
using TinyCsv.Attributes;

namespace DarkStar.Api.Engine.Serialization.Seeds;

[SeedObject("Races")]
[HasHeaderRecord(true)]
[Delimiter(";")]
public class RaceObjectSeedEntity
{
    [Column] public string Name { get; set; } = null!;

    [Column] public string Description { get; set; } = null!;

    [Column(converter: typeof(TileTypeConverter))]
    public int TileType { get; set; }

    [Column] public int Strength { get; set; }
    [Column] public int Dexterity { get; set; }
    [Column] public int Intelligence { get; set; }
    [Column] public int Luck { get; set; }
    [Column] public int MaxHealth { get; set; }
    [Column] public int MaxMana { get; set; }
    [Column] public int Health { get; set; }
    [Column] public int Mana { get; set; }

    [Column] public bool IsVisible { get; set; } = true;
}
