using DarkStar.Database.Entities.Base;

using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.Npc;

[Table(Name = "npc_stats")]
public class NpcStatEntity : BaseStatEntity
{
    public NpcEntity Player { get; set; } = null!;
    public Guid NpcId { get; set; }
}
