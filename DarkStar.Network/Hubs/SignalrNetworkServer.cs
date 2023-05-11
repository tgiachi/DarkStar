using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Interfaces;
using DarkStar.Network.Protocol.Interfaces.Builders;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using DarkStar.Network.Server.Interfaces;
using DarkStar.Network.Session.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DarkStar.Network.Hubs;

public class SignalrNetworkServer : IDarkSunNetworkServer
{
    private readonly ILogger<SignalrNetworkServer> _logger;
    public Task StartAsync() => Task.CompletedTask;
    public Task StopAsync() => Task.CompletedTask;

    private readonly INetworkMessageBuilder _messageBuilder;
    private readonly INetworkSessionManager _sessionManager;
    private readonly IHubContext<SignalrMessageHub> _hubContext;


    public event IDarkSunNetworkServer.MessageReceivedDelegate? OnMessageReceived;
    public event IDarkSunNetworkServer.ClientConnectedMessages? OnClientConnected;
    public event IDarkSunNetworkServer.ClientDisconnectedDelegate? OnClientDisconnected;

    private readonly Dictionary<DarkStarMessageType, INetworkServerMessageListener> _messageListeners = new();

    public SignalrNetworkServer(
        ILogger<SignalrNetworkServer> logger, INetworkMessageBuilder messageBuilder, INetworkSessionManager sessionManager,
        IHubContext<SignalrMessageHub> hubContext
    )
    {
        _logger = logger;
        _messageBuilder = messageBuilder;
        _sessionManager = sessionManager;
        _hubContext = hubContext;
    }

    public async Task OnConnectedClient(string sessionId)
    {
        _logger.LogInformation("Client {IpAddress} connected with sessionId: {SessionId}", sessionId, sessionId);
        var messages = await OnClientConnected?.Invoke(sessionId);
        await SendMessageAsync(sessionId, messages);
    }



    public async Task SendMessageAsync(string sessionId, IDarkStarNetworkMessage message)
    {
        await _hubContext.Clients.Client(sessionId)
            .SendAsync("IncomingMessage", Encoding.UTF8.GetString(_messageBuilder.BuildMessage(message)));
    }

    public async Task SendMessageAsync(string sessionId, List<IDarkStarNetworkMessage> message)
    {
        foreach (var networkMessage in message)
        {
            await _hubContext.Clients.Client(sessionId)
                .SendAsync("IncomingMessage", Encoding.UTF8.GetString(_messageBuilder.BuildMessage(networkMessage)));
        }
    }

    public async Task BroadcastMessageAsync(IDarkStarNetworkMessage message)
    {
        await _hubContext.Clients.All.SendAsync(
            "IncomingMessage",
            Encoding.UTF8.GetString(_messageBuilder.BuildMessage(message))
        );
    }

    public async Task DispatchMessageReceivedAsync(
        string sessionId, DarkStarMessageType messageType, IDarkStarNetworkMessage message
    )
    {
        if (OnMessageReceived != null)
        {
            await OnMessageReceived?.Invoke(sessionId, messageType, message);
        }

        if (_messageListeners.TryGetValue(messageType, out var listener))
        {
            await listener.OnMessageReceivedAsync(sessionId, messageType, message);
        }
    }

    public void RegisterMessageListener(DarkStarMessageType messageType, INetworkServerMessageListener serverMessageListener)
    {
        _messageListeners.Add(messageType, serverMessageListener);
    }
}
