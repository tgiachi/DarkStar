using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;


using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Server;

[NetworkMessage(DarkStarMessageType.ServerVersionResponse)]
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
