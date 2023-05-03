using DarkStar.Database.Entities.Base;
using DarkStar.Database.Entities.Item;
using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.Npc;

[Table(Name = "npc_inventories")]
public class NpcInventoryEntity : BaseEntity
{
    public ItemEntity Item { get; set; } = null!;
    public Guid ItemId { get; set; }
    public NpcEntity Npc { get; set; } = null!;
    public Guid NpcId { get; set; }
    public int Amount { get; set; }
}
