using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Attributes.Seed;
using DarkSun.Api.Engine.Serialization.Seeds.Converters;
using DarkSun.Api.Serialization.Converters;
using DarkSun.Api.World.Types.GameObjects;
using DarkSun.Api.World.Types.Tiles;
using TinyCsv.Attributes;

namespace DarkSun.Api.Engine.Serialization.Seeds
{
    [SeedObject("GameObjects")]
    [HasHeaderRecord(true)]
    [Delimiter(";")]
    public class WorldObjectSeedEntity
    {
        [Column]
        public string Name { get; set; } = null!;
        [Column]
        public string Description { get; set; } = null!;
        [Column(converter: typeof(TileTypeConverter))]
        public TileType TileId { get; set; }
        [Column(converter: typeof(GameObjectTypeConverter))]
        public GameObjectType Type { get; set; }
    }
}
