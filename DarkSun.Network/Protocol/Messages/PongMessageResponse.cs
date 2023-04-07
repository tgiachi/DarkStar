using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Attributes;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;
using MessagePack;

namespace DarkSun.Network.Protocol.Messages
{

    [MessagePackObject(keyAsPropertyName: true)]
    [NetworkMessage(DarkSunMessageType.Pong)]
    public class PongMessageResponse : IDarkSunNetworkMessage
    {
        public long TimeStamp { get; set; }
    }
}
