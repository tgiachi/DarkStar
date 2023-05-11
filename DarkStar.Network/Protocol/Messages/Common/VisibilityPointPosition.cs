using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Common;

[ProtoContract]
public class VisibilityPointPosition : PointPosition
{
    [ProtoMember(1)] public double Visibility { get; set; }
    public VisibilityPointPosition(int x, int y, double visibility) : base(x, y) => Visibility = visibility;

    public static VisibilityPointPosition operator +(VisibilityPointPosition a, VisibilityPointPosition b) =>
        new(a.X + b.X, a.Y + b.Y, a.Visibility + b.Visibility);

    public static VisibilityPointPosition operator -(VisibilityPointPosition a, VisibilityPointPosition b) =>
        new(a.X - b.X, a.Y - b.Y, a.Visibility - b.Visibility);

    public static VisibilityPointPosition New(int x, int y, double visibility) => new(x, y, visibility);

    public override string ToString() => $"X: {X} Y: {Y} Visibility: {Visibility}";
}
