using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Protocol.Types;

using ProtoBuf;

namespace DarkSun.Network.Protocol;

[ProtoContract]
public class NetworkMessage
{
    [ProtoMember(1)]
    public DarkSunMessageType MessageType { get; set; }
    [ProtoMember(2)]
    public byte[] Message { get; set; } = null!;
}
