using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Utils;
using SadRogue.Primitives;

namespace DarkSun.Api.Engine.Utils
{
    public static class RandPointUtils
    {
        public static Point RandomPoint(int width, int height)
        {
            return new Point(RandomUtils.Range(0, width), RandomUtils.Range(0, height));
        }
    }
}
