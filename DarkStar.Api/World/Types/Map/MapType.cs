using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Api.World.Types.Map;

public enum MapType : short
{
    World,
    Dungeon,
    City,
    Town,
    Village,
    Camp,
    Ruins,
    Cave,
    Forest
}
