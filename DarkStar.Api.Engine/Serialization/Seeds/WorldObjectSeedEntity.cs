using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Attributes.Seed;
using DarkStar.Api.Engine.Serialization.Seeds.Converters;
using DarkStar.Api.Serialization.Converters;
using DarkStar.Api.World.Types.GameObjects;
using DarkStar.Api.World.Types.Tiles;

using TinyCsv.Attributes;

namespace DarkStar.Api.Engine.Serialization.Seeds
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

        [Column(converter: typeof(ExtraDataConverter))]
        public Dictionary<string, string> Data { get; set; } = null!;
    }
}
