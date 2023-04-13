using DarkSun.Api.World.Types.Equippable;
using DarkSun.Database.Entities.Base;
using DarkSun.Database.Entities.Item;
using DarkSun.Database.Entities.Player;
using FreeSql.DataAnnotations;

namespace DarkSun.Database.Entities.Npc;

[Table(Name = "npc_equipments")]
public class NpcEquipmentEntity : BaseEntity
{
    public Guid NpcId { get; set; }
    public Guid ItemId { get; set; }
    public NpcEntity Npc { get; set; } = null!;
    public ItemEntity Item { get; set; } = null!;
    public EquipLocationType EquipLocation { get; set; }
}
