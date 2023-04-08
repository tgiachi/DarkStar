using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.World.Types.Npc;
using DarkSun.Api.World.Types.Tiles;
using DarkSun.Database.Entities.Base;
using FreeSql.DataAnnotations;

namespace DarkSun.Database.Entities.Npc
{
    [Table(Name = "npcs")]
    public class NpcEntity : BaseEntity
    {
        public string Name { get; set; } = null!;
        public TileType TileId { get; set; }
        public NpcAlignmentType Alignment { get; set; }
        public NpcType Type { get; set; }
    }
}
