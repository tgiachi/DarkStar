using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.World.Types.Equippable;
using DarkSun.Database.Entities.Base;
using DarkSun.Database.Entities.Item;
using FreeSql.DataAnnotations;

namespace DarkSun.Database.Entities.Player;

[Table(Name = "player_equipments")]
public class PlayerEquipmentEntity : BaseEntity
{
    public Guid PlayerId { get; set; }
    public Guid ItemId { get; set; }
    public PlayerEntity Player { get; set; } = null!;
    public ItemEntity Item { get; set; } = null!;
    public EquipLocationType EquipLocation { get; set; }
}
