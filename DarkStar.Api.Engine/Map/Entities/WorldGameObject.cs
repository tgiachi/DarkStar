using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Map.Entities.Base;
using DarkStar.Api.World.Types.GameObjects;
using DarkStar.Api.World.Types.Map;
using GoRogue;
using GoRogue.GameFramework;
using SadRogue.Primitives;

namespace DarkStar.Api.Engine.Map.Entities;

public class WorldGameObject : BaseGameObject
{
    public short Type { get; set; }

    public WorldGameObject(Point position) : base(position, (int)MapLayer.Objects, true, false)
    {

    }
}
