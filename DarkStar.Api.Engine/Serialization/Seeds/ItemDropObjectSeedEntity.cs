using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Attributes.Seed;
using DarkStar.Api.Engine.Serialization.Seeds.Converters;
using DarkStar.Api.World.Types.Map;

using TinyCsv.Attributes;

namespace DarkStar.Api.Engine.Serialization.Seeds;

[SeedObject("ItemDrops")]
[HasHeaderRecord(true)]
[Delimiter(";")]
public class ItemDropObjectSeedEntity
{
    [Column]
    public string TemplateName { get; set; } = null!;

    [Column(converter: typeof(MapLayerConverter))]
    public MapLayer MapLayer { get; set; }
    [Column]
    public string ItemName { get; set; } = null!;

    [Column]
    public float DropRate { get; set; }
}
