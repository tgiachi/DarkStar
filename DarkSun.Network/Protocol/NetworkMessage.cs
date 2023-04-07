using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Protocol.Types;
using MessagePack;

namespace DarkSun.Network.Protocol
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class NetworkMessage
    {
        public DarkSunMessageType MessageType { get; set; }
        public byte[] Message { get; set; } = null!;
    }
}
