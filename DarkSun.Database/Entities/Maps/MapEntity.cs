using DarkSun.Api.World.Types.Map;
using DarkSun.Database.Entities.Base;
using FreeSql.DataAnnotations;

namespace DarkSun.Database.Entities.Maps;

[Table(Name = "maps")]
public class MapEntity : BaseEntity
{
    public string MapId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public MapType Type { get; set; }
    public string FileName { get; set; } = null!;
}
