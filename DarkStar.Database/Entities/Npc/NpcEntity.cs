using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Database.Entities.Base;
using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.Npc;

[Table(Name = "npcs")]
public class NpcEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public TileType TileId { get; set; }
    public NpcAlignmentType Alignment { get; set; }
    public NpcType Type { get; set; }
    public NpcSubType SubType { get; set; }
    public List<NpcInventoryEntity> Inventory { get; set; } = null!;
    public List<NpcEquipmentEntity> Equipment { get; set; } = null!;
    public Guid StatsId { get; set; }
    public NpcStatEntity Stats { get; set; } = null!;
    public int Gold { get; set; }
}
