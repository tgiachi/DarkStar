using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Attributes.Seed;
using DarkStar.Api.Serialization.Converters;
using DarkStar.Api.World.Types.Tiles;
using TinyCsv.Attributes;

namespace DarkStar.Api.Serialization.TileSets;

[HasHeaderRecord(true)]
[Delimiter(";")]
[SeedObject("tileSetMap")]
public class TileSetMapSerializable
{
    [Column]
    public int Id { get; set; }
    [Column] public string Name { get; set; }

    [Column] public string Category { get; set; }
    [Column] public string SubCategory { get; set; }

    [Column] public string? Tag { get; set; }

    [Column]
    public bool IsTransparent { get; set; }

    public override string ToString() => $"Id: {Id},  IsBlocked: {IsTransparent}";
}
