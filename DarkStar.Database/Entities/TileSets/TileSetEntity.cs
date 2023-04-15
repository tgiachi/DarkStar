using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Database.Entities.Base;
using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.TileSets
{
    [Table(Name = "tile_sets")]
    public class TileSetEntity : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Source { get; set; } = null!;
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public string TileSetMapFileName { get; set; } = null!;
    }
}
