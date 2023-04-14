using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Database.Entities.Base;
using DarkStar.Database.Entities.Item;
using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.Containers;

[Table(Name = "item_container_details")]
public class ItemContainerDetailEntity : BaseEntity
{
    public Guid ItemContainerId { get; set; }
    public ItemContainerEntity ItemContainer { get; set; } = null!;
    public Guid ItemId { get; set; }
    public ItemEntity Item { get; set; } = null!;
}
