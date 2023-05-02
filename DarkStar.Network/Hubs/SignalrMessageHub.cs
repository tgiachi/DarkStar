using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Protocol.Interfaces.Builders;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using DarkStar.Network.Server.Interfaces;
using DarkStar.Network.Session.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DarkStar.Network.Hubs;



public class SignalrMessageHub : Hub
{
    private readonly ILogger<SignalrMessageHub> _logger;
    private readonly INetworkMessageBuilder _messageBuilder;
    private readonly INetworkSessionManager _sessionManager;
    private readonly IDarkSunNetworkServer _networkServer;


    public SignalrMessageHub(
        ILogger<SignalrMessageHub> logger, INetworkMessageBuilder messageBuilder, INetworkSessionManager sessionManager, IDarkSunNetworkServer networkServer
    )
    {
        _logger = logger;
        _messageBuilder = messageBuilder;
        _sessionManager = sessionManager;
        _networkServer = networkServer;
    }
    public async Task SendMessage(string message)
    {
        try
        {
            var incomingMessage = _messageBuilder.ParseMessage(Encoding.UTF8.GetBytes(message));

            await _networkServer.DispatchMessageReceivedAsync(
                Context.ConnectionId,
                incomingMessage.MessageType,
                incomingMessage.Message
            );
        }
        catch (Exception ex)
        {
            _logger.LogError("Error during receive message from sessionId: {Id}: {Ex}", Context.ConnectionId, ex);

        }
    }

    public override async Task OnConnectedAsync()
    {
        _sessionManager.AddSession(Context.ConnectionId);
        if (_networkServer is SignalrNetworkServer signalrNetwork)
        {
            await signalrNetwork.OnConnectedClient(Context.ConnectionId);
        }
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _sessionManager.RemoveSession(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}
