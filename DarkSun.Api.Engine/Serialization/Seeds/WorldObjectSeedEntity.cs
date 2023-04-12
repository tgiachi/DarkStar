using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.World.Types.GameObjects;
using DarkSun.Api.World.Types.Tiles;
using TinyCsv.Attributes;

namespace DarkSun.Api.Engine.Serialization.Seeds
{
    [HasHeaderRecord(true)]
    [Delimiter(";")]
    public class WorldObjectSeedEntity
    {
        [Column]
        public string Name { get; set; } = null!;
        [Column]
        public string Description { get; set; } = null!;
        [Column]
        public TileType TileId { get; set; }
        [Column]
        public GameObjectType Type { get; set; }
    }
}
