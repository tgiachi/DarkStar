using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Common;

[ProtoContract]
public class PointPosition
{
    [ProtoMember(1)] public int X { get; set; }
    [ProtoMember(2)] public int Y { get; set; }

    public PointPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    public PointPosition()
    {
    }

    public static PointPosition operator +(PointPosition a, PointPosition b) => new(a.X + b.X, a.Y + b.Y);

    public static PointPosition operator -(PointPosition a, PointPosition b) => new(a.X - b.X, a.Y - b.Y);

    public static PointPosition New(int x, int y) => new(x, y);

    public override string ToString() => $"X: {X} Y: {Y}";
}
