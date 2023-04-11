using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Database.Entities.Base;
using DarkSun.Database.Entities.Item;
using FreeSql.DataAnnotations;

namespace DarkSun.Database.Entities.Player;

[Table(Name = "player_inventories")]
public class PlayerInventoryEntity : BaseEntity
{
    public ItemEntity Item { get; set; } = null!;
    public Guid ItemId { get; set; }
    public PlayerEntity Player { get; set; } = null!;
    public Guid PlayerId { get; set; }
    public int Amount { get; set; }
}
