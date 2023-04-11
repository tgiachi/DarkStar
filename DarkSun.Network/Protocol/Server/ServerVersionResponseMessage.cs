using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Attributes;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;
using MessagePack;

namespace DarkSun.Network.Protocol.Server;

[NetworkMessage(DarkSunMessageType.ServerVersionResponse)]
[MessagePackObject(true)]
public class ServerVersionResponseMessage : IDarkSunNetworkMessage
{
    public int Minor { get; set; }
    public int Major { get; set; }
    public int Build { get; set; }

    public ServerVersionResponseMessage()
    {
    }

    public ServerVersionResponseMessage(int major, int minor, int build)
    {
        Minor = minor;
        Major = major;
        Build = build;
    }
}
