using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace DarkSun.Network.Protocol.Messages.Common
{
    [MessagePackObject(keyAsPropertyName: true)]
    public struct PointPosition
    {
        public int X { get; set; }
        public int Y { get; set; }

        public PointPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public PointPosition()
        {

        }

        public static PointPosition operator +(PointPosition a, PointPosition b)
        {
            return new PointPosition(a.X + b.X, a.Y + b.Y);
        }

        public static PointPosition operator -(PointPosition a, PointPosition b)
        {
            return new PointPosition(a.X - b.X, a.Y - b.Y);
        }



        public override string ToString()
        {
            return $"X: {X} Y: {Y}";
        }
    }
}
