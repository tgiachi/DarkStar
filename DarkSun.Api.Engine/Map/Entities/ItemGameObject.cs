using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Map.Entities.Base;
using DarkSun.Api.World.Types.Map;
using GoRogue;
using GoRogue.GameFramework;
using SadRogue.Primitives;

namespace DarkSun.Api.Engine.Map.Entities
{
    public class ItemGameObject : BaseGameObject
    {
        public ItemGameObject(Point position) : base(position, (int)MapLayer.Items,  true, false)
        {
        }
    }
}
