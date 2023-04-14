using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Serialization.Converters;
using DarkSun.Api.World.Types.Tiles;
using TinyCsv.Attributes;

namespace DarkSun.Api.Serialization.TileSets
{
    [HasHeaderRecord(true)]
    [Delimiter(";")]
    public class TileSetMapSerializable
    {
        [Column]
        public int Id { get; set; }

        [Column(converter: typeof(TileTypeConverter))]
        public TileType Type { get; set; }

        [Column]
        public bool IsBlocked { get; set; }
    }
}
