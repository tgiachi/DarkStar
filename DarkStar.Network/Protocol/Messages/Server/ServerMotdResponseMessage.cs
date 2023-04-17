using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;

using ProtoBuf;


namespace DarkStar.Network.Protocol.Messages.Server
{

    [ProtoContract]
    [NetworkMessage(DarkStarMessageType.ServerMotdResponse)]
    public class ServerMotdResponseMessage : IDarkStarNetworkMessage
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
