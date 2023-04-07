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
using MessagePack;
using Microsoft.Extensions.Logging;

namespace DarkSun.Network.Protocol.Builders
{
    public class MessagePackMessageBuilder : INetworkMessageBuilder
    {
        private readonly ILogger _logger;
        private readonly Dictionary<DarkSunMessageType, Type> _messageTypes = new();


        public MessagePackMessageBuilder(ILogger<MessagePackMessageBuilder> logger)
        {
            _logger = logger;
            PrepareMessageTypesConversionMap();
        }

        public NetworkMessageData ParseMessage(byte[] buffer)
        {
            _logger.LogDebug("Parsing message buffer of length {Length}", buffer.Length);

            var messageLength = BufferUtils.GetIntFromByteArray(buffer);
            _logger.LogDebug("Message length is {Length}", messageLength);

            var messageBuffer = buffer.Skip(BufferUtils.LengthHeaderSize).Take(messageLength).ToArray();

            var message = MessagePackSerializer.Deserialize<NetworkMessage>(messageBuffer);
            _logger.LogDebug("Message type is {MessageType}", message.MessageType);
            var innerMessage = MessagePackSerializer.Deserialize(_messageTypes[message.MessageType], message.Message) as IDarkSunNetworkMessage;

            return new NetworkMessageData
            {
                MessageType = message.MessageType,
                Message = innerMessage!
            };

        }

        public byte[] BuildMessage<T>(T message) where T : IDarkSunNetworkMessage
        {

            // Message structure is [MessageLength] - [[MessageType]-[MessageContent]]
            var messageType = GetMessageTypeAttribute(message.GetType());
            _logger.LogDebug("Building message buffer for message type: {MessageType}", messageType);
            var messageContent = MessagePackSerializer.Serialize(message);
            _logger.LogDebug("Inner message content length: {MessageContentLength}", messageContent.Length);
            var serializedMessage = MessagePackSerializer.Serialize(new NetworkMessage
            {
                MessageType = messageType,
                Message = messageContent
            });
            _logger.LogDebug("Full message buffer length: {MessageBufferLength}", serializedMessage.Length);
            var fullBufferArray = BufferUtils.Combine(BufferUtils.GetByteArrayFromInt(serializedMessage.Length), serializedMessage);
            _logger.LogDebug("Completed message buffer is {Length}", fullBufferArray.Length);

            return fullBufferArray;
        }

        public int GetMessageLength(byte[] buffer)
        {
            return BufferUtils.GetIntFromByteArray(buffer); 
        }


        private DarkSunMessageType GetMessageTypeAttribute(Type message)
        {
            var attribute = message.GetCustomAttribute<NetworkMessageAttribute>();

            return attribute?.MessageType ?? throw new Exception($"Message {message.Name} does not have a NetworkMessageAttribute");
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
}
