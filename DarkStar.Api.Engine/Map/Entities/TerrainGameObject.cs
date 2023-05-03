using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Map.Entities.Base;
using DarkStar.Api.World.Types.Map;
using GoRogue;
using GoRogue.GameFramework;
using SadRogue.Primitives;

namespace DarkStar.Api.Engine.Map.Entities;

public class TerrainGameObject : BaseGameObject
{
    public TerrainGameObject(Point position, bool isWalkable = true, bool isTransparent = true) : base(
        position,
        (int)MapLayer.Terrain,
        isWalkable,
        isTransparent
    )
    {
    }
}
