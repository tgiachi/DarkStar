using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.World;


[NetworkMessage(DarkStarMessageType.WorldMessageRequest)]
[ProtoContract]
public struct WorldMessageRequestMessage : IDarkStarNetworkMessage
{
    [ProtoMember(1)]
    public string Message { get; set; } = null!;
    [ProtoMember(2)]
    public string Sender { get; set; } = null!;
    [ProtoMember(3)]
    public WorldMessageType MessageType { get; set; }


    public WorldMessageRequestMessage(string message, string sender, WorldMessageType messageType)
    {
        Message = message;
        Sender = sender;
        MessageType = messageType;
    }

    public WorldMessageRequestMessage()
    {

    }
}
