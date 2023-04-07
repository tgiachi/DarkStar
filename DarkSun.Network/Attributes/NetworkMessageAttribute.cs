using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Protocol.Types;

namespace DarkSun.Network.Attributes
{
    public class NetworkMessageAttribute : Attribute
    {
        public DarkSunMessageType MessageType { get; set; }

        public NetworkMessageAttribute(DarkSunMessageType messageType)
        {
            MessageType = messageType;
        }
    }
}
