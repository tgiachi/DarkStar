﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSun.Api.World.Types.Map;

public enum MapLayer : short
{
    Terrain,
    Objects,
    Items,
    Creatures,
    Players,
    Effects,
    Weather,
}

