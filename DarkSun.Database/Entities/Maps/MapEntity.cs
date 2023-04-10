using DarkSun.Api.World.Types.Map;
using DarkSun.Database.Entities.Base;
using FreeSql.DataAnnotations;

namespace DarkSun.Database.Entities.Maps
{
    [Table(Name = "maps")]
    public class MapEntity : BaseEntity
    {
        public Guid MapId { get; set; }
        public string Name { get; set; } = null!;
        public MapType Type { get; set; }
        public string FileName { get; set; } = null!;
    }
}
