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

public class ItemGameObject : BaseGameObject
{
    public string Name { get; set; }
    public ItemGameObject(Point position) : base(position, (int)MapLayer.Items, true, false)
    {
    }
}
