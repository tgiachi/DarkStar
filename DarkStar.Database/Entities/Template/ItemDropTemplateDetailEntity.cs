using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Database.Entities.Base;
using DarkStar.Database.Entities.Item;
using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.Template;

[Table(Name = "item_template_details")]
public class ItemDropTemplateDetailEntity : BaseEntity
{
    public Guid ItemId { get; set; }
    public Guid ItemDropTemplateId { get; set; }
    public ItemEntity Item { get; set; } = null!;
    public ItemDropTemplateEntity ItemDropTemplate { get; set; } = null!;
    public int DropChance { get; set; }
}
