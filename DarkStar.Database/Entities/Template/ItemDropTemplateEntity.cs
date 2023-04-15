using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Database.Entities.Base;
using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.Template;

[Table(Name = "item_template")]
public class ItemDropTemplateEntity : BaseEntity
{
    public string Name { get; set; } = null!;
}
