﻿using System;
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
    public class PlayerGameObject : BaseGameObject
    {
        public PlayerGameObject(Coord position) : base(position, (int)MapLayer.Players, null!, true, false, true)
        {
        }
    }
}
