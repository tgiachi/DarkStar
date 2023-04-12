using DarkSun.Network.Attributes;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;
using ProtoBuf;

namespace DarkSun.Network.Protocol.Messages.Server
{
    [ProtoContract]
    [NetworkMessage(DarkSunMessageType.ServerNameResponse)]
    public class ServerNameResponseMessage : IDarkSunNetworkMessage
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
}
