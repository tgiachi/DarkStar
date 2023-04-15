using DarkStar.Api.World.Types.Equippable;
using DarkStar.Database.Entities.Base;
using DarkStar.Database.Entities.Item;

using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.Npc;

[Table(Name = "npc_equipments")]
public class NpcEquipmentEntity : BaseEntity
{
    public Guid NpcId { get; set; }
    public Guid ItemId { get; set; }
    public NpcEntity Npc { get; set; } = null!;
    public ItemEntity Item { get; set; } = null!;
    public EquipLocationType EquipLocation { get; set; }
}
