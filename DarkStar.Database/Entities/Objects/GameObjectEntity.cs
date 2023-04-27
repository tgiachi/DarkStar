using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.GameObjects;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Database.Entities.Base;
using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.Objects;

[Table(Name = "game_objects")]
public class GameObjectEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public uint TileId { get; set; }
    public short GameObjectType { get; set; }
    public string Data { get; set; } = null!;
}
