using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Protocol.Interfaces.Builders;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;

namespace DarkSun.Network.Data
{
    public class NetworkMessageData
    {
        public DarkSunMessageType MessageType { get; set; }

        public IDarkSunNetworkMessage Message { get; set; } = null!;
    }
}
