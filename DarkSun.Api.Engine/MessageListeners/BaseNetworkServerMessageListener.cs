using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Interfaces.Core;
using DarkSun.Network.Interfaces;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;
using Microsoft.Extensions.Logging;

namespace DarkSun.Api.Engine.MessageListeners;

public class BaseNetworkMessageListener<TMessage> : INetworkServerMessageListener where TMessage : IDarkSunNetworkMessage
{
    protected ILogger Logger { get; }
    protected IDarkSunEngine Engine { get; }

    public BaseNetworkMessageListener(ILogger<BaseNetworkMessageListener<TMessage>> logger, IDarkSunEngine engine)
    {
        Logger = logger;
        Engine = engine;
    }

    public async Task OnMessageReceivedAsync(Guid sessionId, DarkSunMessageType messageType, IDarkSunNetworkMessage message)
    {
        try
        {
            var messages = await OnMessageReceivedAsync(sessionId, messageType, (TMessage)message);
            if (messages != null! || messages!.Count > 0)
            {
                await Engine.NetworkServer.SendMessageAsync(sessionId, messages);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while processing message {MessageType} for session {SessionId}", messageType,
                sessionId);
        }
    }

    public virtual Task<List<IDarkSunNetworkMessage>> OnMessageReceivedAsync(Guid sessionId, DarkSunMessageType messageType, TMessage message)
    {
        return Task.FromResult(new List<IDarkSunNetworkMessage>());
    }

    protected List<IDarkSunNetworkMessage> SingleMessage(IDarkSunNetworkMessage message)
    {
        return new List<IDarkSunNetworkMessage>() { message };
    }

    protected List<IDarkSunNetworkMessage> MultipleMessages(params IDarkSunNetworkMessage[] message)
    {
        var mess = new List<IDarkSunNetworkMessage>();
        mess.AddRange(message);

        return mess;
    }

    protected List<IDarkSunNetworkMessage> EmptyMessage()
    {
        return new List<IDarkSunNetworkMessage>();
    }
}
