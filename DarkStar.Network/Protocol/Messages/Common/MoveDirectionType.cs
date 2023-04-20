using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Common;

[ProtoContract]
public enum MoveDirectionType : byte
{
    North,
    South,
    East,
    West,
    NorthEast,
    NorthWest,
    SouthEast,
    SouthWest
}
