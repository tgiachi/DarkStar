using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Map.Entities.Base;
using DarkSun.Api.World.Types.Map;
using GoRogue;
using GoRogue.GameFramework;

namespace DarkSun.Api.Engine.Map.Entities
{
    public class TerrainGameObject : BaseGameObject
    {
        public TerrainGameObject(Coord position, bool isWalkable = true, bool isTransparent = true) : base(position, (int)MapLayer.Terrain, null!, true, isWalkable, isTransparent)
        {
        }
    }
}
