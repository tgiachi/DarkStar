using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Common
{
    [ProtoContract]
    public enum PlayerMoveDirectionType : byte
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
}
