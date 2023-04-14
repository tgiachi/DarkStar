using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Protocol.Messages.Common;
using SadRogue.Primitives;

namespace DarkSun.Api.Engine.Utils
{
    public static class PointConverterEx
    {
        public static PointPosition ToPointPosition(this Point point)
        {
            return new PointPosition(point.X, point.Y);
        }
        public static Point ToPoint(this PointPosition pointPosition)
        {
            return new Point(pointPosition.X, pointPosition.Y);
        }
    }
}
