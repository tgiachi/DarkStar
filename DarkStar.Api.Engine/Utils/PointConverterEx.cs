using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Protocol.Messages.Common;
using SadRogue.Primitives;

namespace DarkStar.Api.Engine.Utils
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
