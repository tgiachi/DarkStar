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
    public class NpcGameObject : BaseGameObject
    {
        public NpcGameObject(Coord position) : base(position, (int)MapLayer.Creatures, null!, false, true, false)
        {
        }
    }
}
