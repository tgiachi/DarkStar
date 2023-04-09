using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.World.Types.GameObjects;
using DarkSun.Api.World.Types.Tiles;
using DarkSun.Database.Entities.Base;
using FreeSql.DataAnnotations;

namespace DarkSun.Database.Entities.Objects
{
    [Table(Name = "game_objects")]
    public class GameObjectEntity : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TileType TileId { get; set; }
        public GameObjectType Type { get; set; }
    }
}
