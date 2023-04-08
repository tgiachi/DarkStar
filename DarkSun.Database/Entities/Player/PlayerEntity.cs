using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.World.Types.Tiles;
using DarkSun.Database.Entities.Account;
using DarkSun.Database.Entities.Base;
using FreeSql.DataAnnotations;

namespace DarkSun.Database.Entities.Player
{
    [Table(Name = "players")]
    public class PlayerEntity : BaseEntity
    {
        public Guid UserId { get; set; }
        public AccountEntity User { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int X { get; set; }
        public int Y { get; set; }
        public Guid MapId { get; set; }
        public TileType TileId { get; set; }
        public PlayerStatEntity Stats { get; set; } = null!;
        public List<PlayerInventory> Inventory { get; set; } = null!;
        public int Gold { get; set; }
    }
}
