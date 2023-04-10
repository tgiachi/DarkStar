using DarkSun.Database.Entities.Base;
using DarkSun.Database.Entities.Player;
using FreeSql.DataAnnotations;

namespace DarkSun.Database.Entities.Npc
{
    [Table(Name = "npc_stats")]
    public class NpcStatEntity : BaseStatEntity
    {
        public NpcEntity Player { get; set; } = null!;
        public Guid NpcId { get; set; }
    }
}
