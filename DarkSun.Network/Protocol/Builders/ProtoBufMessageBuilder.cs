using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Utils;
using DarkSun.Network.Attributes;
using DarkSun.Network.Data;
using DarkSun.Network.Protocol.Interfaces.Builders;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;

using Microsoft.Extensions.Logging;
using ProtoBuf;

namespace DarkSun.Network.Protocol.Builders;

public class ProtoBufMessageBuilder : INetworkMessageBuilder
{
    private readonly ILogger _logger;
    private readonly Dictionary<DarkSunMessageType, Type> _messageTypes = new();
    private readonly byte[] _separatorBytes = new byte[] { 0xff, 0xff, 0xff };
    // private readonly IFormatterResolver _formatterResolver;

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

        return new NetworkMessageData { MessageType = message.MessageType, Message = innerMessage as IDarkSunNetworkMessage };
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


    private DarkSunMessageType GetMessageTypeAttribute(Type message)
    {
        var attribute = message.GetCustomAttribute<NetworkMessageAttribute>();

        return attribute?.MessageType ??
               throw new Exception($"Message {message.Name} does not have a NetworkMessageAttribute");
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
