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

[NetworkMessage(DarkStarMessageType.PlayerCreateResponse)]
[ProtoContract]
public class PlayerCreateResponseMessage : IDarkStarNetworkMessage
{
    [ProtoMember(1)]
    public bool Success { get; set; }

    [ProtoMember(2)]
    public Guid PlayerId { get; set; }

    public PlayerCreateResponseMessage() { }

    public PlayerCreateResponseMessage(bool success, Guid playerId)
    {
        Success = success;
        PlayerId = playerId;
    }

}
