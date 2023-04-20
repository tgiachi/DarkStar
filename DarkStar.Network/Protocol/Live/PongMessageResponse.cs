using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;

using ProtoBuf;


namespace DarkStar.Network.Protocol.Live;

[ProtoContract]
[NetworkMessage(DarkStarMessageType.Pong)]
public class PongMessageResponse : IDarkStarNetworkMessage
{
    [ProtoMember(1)]
    public long TimeStamp { get; set; }
}
