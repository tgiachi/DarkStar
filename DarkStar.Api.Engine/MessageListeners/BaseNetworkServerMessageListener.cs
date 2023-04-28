using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Network.Interfaces;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using Microsoft.Extensions.Logging;

namespace DarkStar.Api.Engine.MessageListeners;

public class BaseNetworkMessageListener<TMessage> : INetworkServerMessageListener where TMessage : IDarkStarNetworkMessage
{
    protected ILogger Logger { get; }
    protected IDarkSunEngine Engine { get; }

    public BaseNetworkMessageListener(ILogger<BaseNetworkMessageListener<TMessage>> logger, IDarkSunEngine engine)
    {
        Logger = logger;
        Engine = engine;
    }

    public async Task OnMessageReceivedAsync(string sessionId, DarkStarMessageType messageType, IDarkStarNetworkMessage message)
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

    public virtual Task<List<IDarkStarNetworkMessage>> OnMessageReceivedAsync(string sessionId, DarkStarMessageType messageType, TMessage message)
    {
        return Task.FromResult(new List<IDarkStarNetworkMessage>());
    }

    protected List<IDarkStarNetworkMessage> SingleMessage(IDarkStarNetworkMessage message)
    {
        return new List<IDarkStarNetworkMessage>() { message };
    }

    protected List<IDarkStarNetworkMessage> MultipleMessages(params IDarkStarNetworkMessage[] message)
    {
        var mess = new List<IDarkStarNetworkMessage>();
        mess.AddRange(message);

        return mess;
    }

    protected List<IDarkStarNetworkMessage> EmptyMessage()
    {
        return new List<IDarkStarNetworkMessage>();
    }
}
