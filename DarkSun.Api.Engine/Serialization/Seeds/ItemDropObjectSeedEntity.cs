using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Attributes.Seed;
using DarkSun.Api.Engine.Serialization.Seeds.Converters;
using DarkSun.Api.World.Types.Map;
using TinyCsv.Attributes;

namespace DarkSun.Api.Engine.Serialization.Seeds
{
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
}
