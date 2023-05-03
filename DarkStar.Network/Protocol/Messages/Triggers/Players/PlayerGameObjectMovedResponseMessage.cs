using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Players;

[NetworkMessage(DarkStarMessageType.PlayerGameObjectMovedResponse)]
[ProtoContract]
public struct PlayerGameObjectMovedResponseMessage : IDarkStarNetworkMessage
{
    [ProtoMember(1)] public string MapId { get; set; }

    [ProtoMember(2)] public string PlayerId { get; set; }

    [ProtoMember(3)] public PointPosition Position { get; set; }

    public PlayerGameObjectMovedResponseMessage(string mapId, string playerId, PointPosition position)
    {
        MapId = mapId;
        PlayerId = playerId;
        Position = position;
    }

    public PlayerGameObjectMovedResponseMessage()
    {
    }
}
