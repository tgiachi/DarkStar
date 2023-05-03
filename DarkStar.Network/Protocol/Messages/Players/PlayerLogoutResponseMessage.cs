using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Players;

[ProtoContract]
[NetworkMessage(DarkStarMessageType.PlayerLogoutResponse)]
public class PlayerLogoutResponseMessage : IDarkStarNetworkMessage
{
    public bool Success { get; set; }

    public PlayerLogoutResponseMessage(bool success) => Success = success;

    public PlayerLogoutResponseMessage()
    {
    }
}
