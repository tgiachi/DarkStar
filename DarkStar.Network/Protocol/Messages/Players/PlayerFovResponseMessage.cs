using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Players;

[ProtoContract]
[NetworkMessage(DarkStarMessageType.PlayerFovResponse)]
public struct PlayerFovResponseMessage : IDarkStarNetworkMessage
{
    public List<VisibilityPointPosition> VisiblePositions { get; set; } = new();

    public PlayerFovResponseMessage(List<VisibilityPointPosition> visiblePositions) => VisiblePositions = visiblePositions;
}
