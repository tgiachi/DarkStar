using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Players;

[NetworkMessage(DarkStarMessageType.PlayerInventoryRequest)]
[ProtoContract]
public class PlayerInventoryRequestMessage : IDarkStarNetworkMessage
{
}
