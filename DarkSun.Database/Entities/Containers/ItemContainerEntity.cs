using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Database.Entities.Base;
using DarkSun.Database.Entities.Player;
using FreeSql.DataAnnotations;

namespace DarkSun.Database.Entities.Containers
{

    [Table(Name = "item_containers")]
    public class ItemContainerEntity : BaseEntity
    {
        public string Name { get; set; } = null!;
        public Guid? PlayerId { get; set; }
        public PlayerEntity? Player { get; set; }
    }
}
