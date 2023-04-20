using DarkStar.Api.World.Types.Map;
using DarkStar.Database.Entities.Base;
using FreeSql.DataAnnotations;

namespace DarkStar.Database.Entities.Maps;

[Table(Name = "maps")]
public class MapEntity : BaseEntity
{
    public string MapId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public MapType Type { get; set; }
    public string FileName { get; set; } = null!;
}
