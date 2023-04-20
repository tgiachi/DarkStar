using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Protocol.Types;

namespace DarkStar.Api.Engine.Attributes.Network;

[AttributeUsage(AttributeTargets.Class)]
public class NetworkMessageListenerAttribute : Attribute
{
    public DarkStarMessageType MessageType { get; set; }

    public NetworkMessageListenerAttribute(DarkStarMessageType messageType)
    {
        MessageType = messageType;
    }
}
