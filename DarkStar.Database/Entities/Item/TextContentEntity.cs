using DarkStar.Database.Entities.Base;
using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.Item;

[Table(Name = "text_contents")]
public class TextContentEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Content { get; set; } = null!;
}
