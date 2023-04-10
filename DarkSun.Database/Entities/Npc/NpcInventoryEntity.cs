using DarkSun.Database.Entities.Base;
using DarkSun.Database.Entities.Item;
using DarkSun.Database.Entities.Player;
using FreeSql.DataAnnotations;

namespace DarkSun.Database.Entities.Npc
{
    [Table(Name = "npc_inventories")]
    public class NpcInventoryEntity : BaseEntity
    {
        public ItemEntity Item { get; set; } = null!;
        public Guid ItemId { get; set; }
        public NpcEntity Npc { get; set; } = null!;
        public Guid NpcId { get; set; }
        public int Amount { get; set; }
    }
}
