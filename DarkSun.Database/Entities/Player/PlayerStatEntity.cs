using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Database.Entities.Base;
using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.Player;

[Table(Name = "player_stats")]
public class PlayerStatEntity : BaseStatEntity
{
    public PlayerEntity Player { get; set; } = null!;
    public Guid PlayerId { get; set; }


}
