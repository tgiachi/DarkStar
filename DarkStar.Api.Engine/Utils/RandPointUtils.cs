using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Utils;
using SadRogue.Primitives;

namespace DarkStar.Api.Engine.Utils;

public static class RandPointUtils
{
    public static Point RandomPoint(int width, int height)
    {
        return new Point(RandomUtils.Range(0, width), RandomUtils.Range(0, height));
    }
}
