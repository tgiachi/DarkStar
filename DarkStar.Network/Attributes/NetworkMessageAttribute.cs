using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Protocol.Types;

namespace DarkStar.Network.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class NetworkMessageAttribute : Attribute
{
    public DarkStarMessageType MessageType { get; set; }

    public NetworkMessageAttribute(DarkStarMessageType messageType) => MessageType = messageType;
}
