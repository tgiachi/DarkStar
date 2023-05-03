using DarkStar.Network.Protocol.Interfaces.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DarkStar.Network.Data;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Api.Utils;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Types;
using Humanizer;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProtoBuf;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DarkStar.Network.Protocol.Builders;

public class JsonMessageBuilder : INetworkMessageBuilder
{
    private readonly ILogger _logger;
    private readonly Dictionary<DarkStarMessageType, Type> _messageTypes = new();
    public byte[] GetMessageSeparators { get; } = null!;
    public int GetMessageLength(byte[] buffer) => 0;

    public JsonMessageBuilder(ILogger<JsonMessageBuilder> logger)
    {
        _logger = logger;
        PrepareMessageTypesConversionMap();
    }


    public NetworkMessageData ParseMessage(byte[] buffer)
    {
        _logger.LogDebug("Parsing message buffer of length {Length}", buffer.Length.Bytes());

        var message = JsonSerializer.Deserialize<NetworkMessage>(buffer);
        _logger.LogDebug("Message type is {MessageType}", message.MessageType);
        var innerMessage = JsonConvert.DeserializeObject(
            Encoding.UTF8.GetString(message.Message),
            _messageTypes[message.MessageType]
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
                var options = new JsonSerializerOptions
                {
                    MaxDepth = 1000
                };

                var innerMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var netMessage =
                    new NetworkMessage() { Message = innerMessage, MessageType = messageType.MessageType };
                var fullMessage = JsonSerializer.Serialize(netMessage);


                _logger.LogDebug(
                    "Sending message type: {Type} - Length: {BufferSize}",
                    messageType.MessageType,
                    fullMessage.Length.Bytes()
                );
                return Encoding.UTF8.GetBytes(fullMessage);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        throw new Exception($"Missing attribute [NetworkMessageAttribute] on message ${typeof(T)}");
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
