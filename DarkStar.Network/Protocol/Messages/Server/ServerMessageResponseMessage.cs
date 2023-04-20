using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Server;

[ProtoContract]
[NetworkMessage(DarkStarMessageType.ServerMessageResponse)]
public struct ServerMessageResponseMessage
{
    [ProtoMember(1)]
    public string Message { get; set; } = null!;

    [ProtoMember(2)]
    public ServerMessageType Type { get; set; }

    public ServerMessageResponseMessage(string message, ServerMessageType type)
    {
        Message = message;
        Type = type;
    }

    public ServerMessageResponseMessage()
    {

    }

}


[ProtoContract]
public enum ServerMessageType
{
    Announcement,
    Information,
}
