using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;

using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Server;

[ProtoContract]
[NetworkMessage(DarkStarMessageType.ServerNameResponse)]
public class ServerNameResponseMessage : IDarkStarNetworkMessage
{
    [ProtoMember(1)]
    public string ServerName { get; set; }

    public ServerNameResponseMessage()
    {
        ServerName = string.Empty;
    }

    public ServerNameResponseMessage(string serverName)
    {
        ServerName = serverName;
    }
}
