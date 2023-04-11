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

public class BaseNetworkMessageListener<TMessage> : INetworkMessageListener where TMessage : IDarkSunNetworkMessage
{
    protected ILogger Logger { get; }
    protected IDarkSunEngine Engine { get; }

    public BaseNetworkMessageListener(ILogger<BaseNetworkMessageListener<TMessage>> logger, IDarkSunEngine engine)
    {
        Logger = logger;
        Engine = engine;
    }

    public Task OnMessageReceivedAsync(Guid sessionId, DarkSunMessageType messageType, IDarkSunNetworkMessage message)
    {
        try
        {
            return OnMessageReceivedAsync(sessionId, messageType, (TMessage)message);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while processing message {MessageType} for session {SessionId}", messageType,
                sessionId);
        }

        return Task.CompletedTask;
    }

    public virtual Task OnMessageReceivedAsync(Guid sessionId, DarkSunMessageType messageType, TMessage message)
    {
        return Task.CompletedTask;
    }
}
