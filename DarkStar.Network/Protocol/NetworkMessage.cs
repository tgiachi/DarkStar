using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Protocol.Types;

using ProtoBuf;

namespace DarkStar.Network.Protocol;

[ProtoContract]
public class NetworkMessage
{
    [ProtoMember(1)]
    public DarkStarMessageType MessageType { get; set; }
    [ProtoMember(2)]
    public byte[] Message { get; set; } = null!;
}
