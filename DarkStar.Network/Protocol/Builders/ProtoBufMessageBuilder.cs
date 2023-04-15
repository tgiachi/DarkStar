using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Utils;
using DarkStar.Network.Attributes;
using DarkStar.Network.Data;
using DarkStar.Network.Protocol;
using DarkStar.Network.Protocol.Interfaces.Builders;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;

using Microsoft.Extensions.Logging;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Builders;

public class ProtoBufMessageBuilder : INetworkMessageBuilder
{
    private readonly ILogger _logger;
    private readonly Dictionary<DarkStarMessageType, Type> _messageTypes = new();
    private readonly byte[] _separatorBytes = { 0xff, 0xff, 0xff };

    public byte[] GetMessageSeparators => _separatorBytes;

    public ProtoBufMessageBuilder(ILogger<ProtoBufMessageBuilder> logger)
    {
        _logger = logger;
        PrepareMessageTypesConversionMap();
    }


    public NetworkMessageData ParseMessage(byte[] buffer)
    {
        _logger.LogDebug("Parsing message buffer of length {Length}", buffer.Length);

        var messageBuffer = buffer.Take(buffer.Length - _separatorBytes.Length).ToArray();

        var message = Serializer.Deserialize<NetworkMessage>(new ReadOnlyMemory<byte>(messageBuffer));
        _logger.LogDebug("Message type is {MessageType}", message.MessageType);
        var innerMessage = Serializer.Deserialize(_messageTypes[message.MessageType],
            new MemoryStream(message.Message));

        return new NetworkMessageData { MessageType = message.MessageType, Message = (innerMessage as IDarkSunNetworkMessage)! };
    }

    public byte[] BuildMessage<T>(T message) where T : IDarkSunNetworkMessage
    {
        var messageType = message.GetType().GetCustomAttribute<NetworkMessageAttribute>();

        if (messageType != null)
        {
            var innerMessageStream = new MemoryStream();
            var messageStream = new MemoryStream();
            Serializer.Serialize(innerMessageStream, message);
            var innerMessageBuffer = innerMessageStream.GetBuffer();
            innerMessageBuffer = innerMessageBuffer.Take((int)innerMessageStream.Length).ToArray();

            var netMessage =
                new NetworkMessage() { Message = innerMessageBuffer, MessageType = messageType.MessageType };
            Serializer.Serialize(messageStream, netMessage);

            var netMessageBuffer = messageStream.GetBuffer();
            netMessageBuffer = netMessageBuffer.Take((int)messageStream.Length).ToArray();
            return
                new ReadOnlyMemory<byte>(netMessageBuffer.Concat(_separatorBytes).ToArray()).ToArray();
        }

        throw new Exception($"Missing attribute [NetworkMessageAttribute] on message ${typeof(T)}");
    }

    public int GetMessageLength(byte[] buffer)
    {
        return BufferUtils.GetIntFromByteArray(buffer);
    }


    private void PrepareMessageTypesConversionMap()
    {
        foreach (var type in AssemblyUtils.GetAttribute<NetworkMessageAttribute>())
        {
            var attribute = type.GetCustomAttribute<NetworkMessageAttribute>();
            _messageTypes.Add(attribute!.MessageType, type);
        }
    }
}
