using DarkStar.Network.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Interfaces;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using DarkStar.Network.Protocol.Interfaces.Builders;
using DarkStar.Network.Session.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DarkStar.Network.Server;
public class SignalrNetworkServer : IDarkSunNetworkServer
{
    public event IDarkSunNetworkServer.MessageReceivedDelegate? OnMessageReceived;
    public event IDarkSunNetworkServer.ClientConnectedMessages? OnClientConnected;
    public event IDarkSunNetworkServer.ClientDisconnectedDelegate? OnClientDisconnected;

    public SignalrNetworkServer(ILogger<SignalrNetworkServer> logger,
        INetworkSessionManager sessionManager,
        INetworkMessageBuilder messageBuilder, IHubContext hubContext)
    {
        
    }

    public Task StartAsync() => throw new NotImplementedException();

    public Task StopAsync() => throw new NotImplementedException();

    public Task SendMessageAsync(Guid sessionId, IDarkStarNetworkMessage message) => throw new NotImplementedException();

    public Task SendMessageAsync(Guid sessionId, List<IDarkStarNetworkMessage> message) => throw new NotImplementedException();

    public Task BroadcastMessageAsync(IDarkStarNetworkMessage message) => throw new NotImplementedException();

    public Task DispatchMessageReceivedAsync(Guid sessionId, DarkStarMessageType messageType, IDarkStarNetworkMessage message) => throw new NotImplementedException();

    public void RegisterMessageListener(DarkStarMessageType messageType, INetworkServerMessageListener serverMessageListener)
    {
        throw new NotImplementedException();
    }

    
}
