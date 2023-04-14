using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.World.Types.Tiles;
using DarkSun.Database.Entities.Base;
using FreeSql.DataAnnotations;

namespace DarkSun.Database.Entities.TileSets
{
    [Table(Name = "tile_set_map")]
    public class TileSetMapEntity : BaseEntity
    {
        public Guid TileSetId { get; set; }
        public TileSetEntity TileSet { get; set; } = null!;
        public int TileId { get; set; }
        public TileType TileType { get; set; }

        public bool IsBlocked { get; set; }
    }
}
