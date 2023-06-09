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
using Humanizer;
using Microsoft.Extensions.Logging;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Builders;

public class ProtoBufMessageBuilder : INetworkMessageBuilder
{
    private readonly ILogger _logger;
    private readonly Dictionary<DarkStarMessageType, Type> _messageTypes = new();

    /// <summary>
    /// Each message is separated by this string
    /// </summary>
    private readonly byte[] _separatorBytes = "end_msg"u8.ToArray();

    public byte[] GetMessageSeparators => _separatorBytes;

    public ProtoBufMessageBuilder(ILogger<ProtoBufMessageBuilder> logger)
    {
        _logger = logger;
        PrepareMessageTypesConversionMap();
    }


    public NetworkMessageData ParseMessage(byte[] buffer)
    {
        _logger.LogDebug("Parsing message buffer of length {Length}", buffer.Length.Bytes());

        var messageBuffer = buffer.Take(buffer.Length - _separatorBytes.Length).ToArray();

        var message = Serializer.Deserialize<NetworkMessage>(new ReadOnlyMemory<byte>(messageBuffer));
        _logger.LogDebug("Message type is {MessageType}", message.MessageType);
        var innerMessage = Serializer.Deserialize(
            _messageTypes[message.MessageType],
            new MemoryStream(message.Message)
        );

        return new NetworkMessageData
            { MessageType = message.MessageType, Message = (innerMessage as IDarkStarNetworkMessage)! };
    }

    public byte[] BuildMessage<T>(T message) where T : IDarkStarNetworkMessage
    {
        var messageType = message.GetType().GetCustomAttribute<NetworkMessageAttribute>();

        if (messageType != null)
        {
            try
            {
                using var innerMessageStream = new MemoryStream();
                using var messageStream = new MemoryStream();
                Serializer.Serialize(innerMessageStream, message);
                var innerMessageBuffer = innerMessageStream.GetBuffer();
                innerMessageBuffer = innerMessageBuffer.Take((int)innerMessageStream.Length).ToArray();

                var netMessage =
                    new NetworkMessage() { Message = innerMessageBuffer, MessageType = messageType.MessageType };
                Serializer.Serialize(messageStream, netMessage);

                var netMessageBuffer = messageStream.GetBuffer();
                netMessageBuffer = netMessageBuffer.Take((int)messageStream.Length).ToArray();
                var fullMessage = netMessageBuffer.Concat(_separatorBytes).ToArray();

                _logger.LogDebug(
                    "Sending message type: {Type} - Length: {BufferSize}",
                    messageType.MessageType,
                    fullMessage.Length.Bytes()
                );
                return
                    new ReadOnlyMemory<byte>(fullMessage).ToArray();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        throw new Exception($"Missing attribute [NetworkMessageAttribute] on message ${typeof(T)}");
    }

    public int GetMessageLength(byte[] buffer) => BufferUtils.GetIntFromByteArray(buffer);


    private void PrepareMessageTypesConversionMap()
    {
        foreach (var type in AssemblyUtils.GetAttribute<NetworkMessageAttribute>())
        {
            var attribute = type.GetCustomAttribute<NetworkMessageAttribute>();
            _messageTypes.Add(attribute!.MessageType, type);
        }
    }
}
