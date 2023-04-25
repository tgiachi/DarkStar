using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Database.Entities.Account;
using DarkStar.Database.Entities.Base;
using DarkStar.Database.Entities.Races;
using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.Player;

[Table(Name = "players")]
public class PlayerEntity : BaseEntity
{
    public Guid AccountId { get; set; }
    public AccountEntity Account { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Guid RaceId { get; set; }
    public RaceEntity Race { get; set; } = null!;
    public int X { get; set; }
    public int Y { get; set; }
    public string MapId { get; set; } = null!;
    public int TileId { get; set; }
    public Guid StatsId { get; set; }
    public PlayerStatEntity Stats { get; set; } = null!;
    public List<PlayerInventoryEntity> Inventory { get; set; } = null!;
    public List<PlayerEquipmentEntity> Equipment { get; set; } = null!;
    public int Gold { get; set; }
    public bool IsDead { get; set; }
}
