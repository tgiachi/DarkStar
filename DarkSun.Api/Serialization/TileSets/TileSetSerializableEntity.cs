using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSun.Api.Serialization.TileSets
{
    public class TileSetSerializableEntity
    {
        public string Name { get; set; } = null!;
        public string Source { get; set; } = null!;
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public string TileSetMapFileName { get; set; } = null!;
    }
}
