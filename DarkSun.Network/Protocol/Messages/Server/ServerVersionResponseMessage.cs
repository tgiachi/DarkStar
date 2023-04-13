using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Attributes;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;

using ProtoBuf;

namespace DarkSun.Network.Protocol.Messages.Server;

[NetworkMessage(DarkSunMessageType.ServerVersionResponse)]
[ProtoContract]
public class ServerVersionResponseMessage : IDarkSunNetworkMessage
{
    [ProtoMember(1)]
    public int Minor { get; set; }
    [ProtoMember(2)]
    public int Major { get; set; }
    [ProtoMember(3)]
    public int Build { get; set; }

    public ServerVersionResponseMessage()
    {
    }

    public ServerVersionResponseMessage(int minor, int major, int build)
    {
        Minor = minor;
        Major = major;
        Build = build;
    }
}
