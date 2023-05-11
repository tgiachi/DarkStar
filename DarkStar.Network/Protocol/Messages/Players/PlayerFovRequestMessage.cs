using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Players;


[NetworkMessage(DarkStarMessageType.PlayerFovRequest)]
[ProtoContract]
public class PlayerFovRequestMessage : IDarkStarNetworkMessage
{

}
