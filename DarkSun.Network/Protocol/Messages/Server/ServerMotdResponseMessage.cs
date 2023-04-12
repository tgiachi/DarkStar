using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Attributes;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;
using ProtoBuf;


namespace DarkSun.Network.Protocol.Messages.Server
{

    [ProtoContract]
    [NetworkMessage(DarkSunMessageType.ServerMotdResponse)]
    public class ServerMotdResponseMessage : IDarkSunNetworkMessage
    {
        [ProtoMember(1)]
        public string Motd { get; set; } = null!;
        public ServerMotdResponseMessage() { }

        public ServerMotdResponseMessage(string motd)
        {
            Motd = motd;
        }
    }
}
