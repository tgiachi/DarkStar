using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Database.Entities.Base;
using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.TileSets;

[Table(Name = "tile_set_map")]
public class TileSetMapEntity : BaseEntity
{
    public Guid TileSetId { get; set; }
    public TileSetEntity TileSet { get; set; } = null!;
    public int TileId { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string SubCategory { get; set; }
    public string? Tag { get; set; }
    public bool IsTransparent { get; set; }
}
