using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Equippable;
using DarkStar.Database.Entities.Base;
using DarkStar.Database.Entities.Item;
using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.Player;

[Table(Name = "player_equipments")]
public class PlayerEquipmentEntity : BaseEntity
{
    public Guid PlayerId { get; set; }
    public Guid ItemId { get; set; }
    public PlayerEntity Player { get; set; } = null!;
    public ItemEntity Item { get; set; } = null!;
    public EquipLocationType EquipLocation { get; set; }
}
