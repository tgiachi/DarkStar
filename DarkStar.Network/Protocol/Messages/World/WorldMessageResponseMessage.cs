using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.World;

[NetworkMessage(DarkStarMessageType.WorldMessageResponse)]
[ProtoContract]
public class WorldMessageResponseMessage : IDarkStarNetworkMessage
{
    public string Message { get; set; } = null!;
    public string SenderId { get; set; } = null!;
    public string SenderName { get; set; } = null!;
    public WorldMessageType MessageType { get; set; }

    public WorldMessageResponseMessage(string message, string senderId, string senderName,
        WorldMessageType messageType)
    {
        Message = message;
        SenderId = senderId;
        SenderName = senderName;
        MessageType = messageType;
    }

    public WorldMessageResponseMessage()
    {
    }
}
