using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Protocol.Types;

namespace DarkSun.Api.Engine.Attributes.Network;

[AttributeUsage(AttributeTargets.Class)]
public class NetworkMessageListenerAttribute : Attribute
{
    public DarkSunMessageType MessageType { get; set; }

    public NetworkMessageListenerAttribute(DarkSunMessageType messageType)
    {
        MessageType = messageType;
    }
}
