using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.World.Types.Tiles;
using GoRogue;
using GoRogue.GameFramework;
using SadRogue.Primitives;

namespace DarkSun.Api.Engine.Map.Entities.Base
{
    public class BaseGameObject : GameObject
    {
        public TileType Tile { get; set; }
        public Guid ObjectId { get; set; }

        public BaseGameObject(Point position, int layer, bool isStatic = false, bool isWalkable = true, bool isTransparent = true) : base(position, layer, isStatic, isWalkable)
        {
        }
    }
}
