using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Data;
using DarkStar.Network.Interfaces;
using DarkStar.Network.Protocol.Interfaces.Builders;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using DarkStar.Network.Server.Interfaces;
using DarkStar.Network.Session.Interfaces;
using Microsoft.Extensions.Logging;
using NetCoreServer;

namespace DarkStar.Network.Server;

public class TcpNetworkServer : TcpServer, IDarkSunNetworkServer
{
    private readonly ILogger _logger;
    private readonly INetworkSessionManager _sessionManager;
    private readonly INetworkMessageBuilder _messageBuilder;
    private readonly DarkStarNetworkServerConfig _darkStarNetworkServerConfig;

    public event IDarkSunNetworkServer.MessageReceivedDelegate? OnMessageReceived;
    public event IDarkSunNetworkServer.ClientConnectedMessages? OnClientConnected;
    public event IDarkSunNetworkServer.ClientDisconnectedDelegate? OnClientDisconnected;

    private readonly Dictionary<DarkStarMessageType, INetworkServerMessageListener> _messageListeners = new();

    public TcpNetworkServer(ILogger<TcpNetworkServer> logger,
        INetworkSessionManager sessionManager,
        INetworkMessageBuilder messageBuilder,
        DarkStarNetworkServerConfig darkStarNetworkServerConfig) : base(darkStarNetworkServerConfig.Address,
        darkStarNetworkServerConfig.Port)
    {
        _logger = logger;
        _sessionManager = sessionManager;
        _messageBuilder = messageBuilder;
        _darkStarNetworkServerConfig = darkStarNetworkServerConfig;

        OptionReceiveBufferSize = 1024 * 10;
        OptionSendBufferSize = 1024 * 10;
    }

    protected override TcpSession CreateSession()
    {
        return new DarkSunTcpSession(this, _messageBuilder, this);
    }

    protected override async void OnConnected(TcpSession session)
    {
        _logger.LogInformation("Client {IpAddress} connected with sessionId: {SessionId}",
            session.Socket.RemoteEndPoint, session.Id);

        _sessionManager.AddSession(session.Id);

        var messagesToSend = await OnClientConnected?.Invoke(session.Id)!;
        if (messagesToSend != null)
        {
            await SendMessageAsync(session.Id, messagesToSend);
        }

        base.OnConnected(session);
    }

    protected override void OnDisconnected(TcpSession session)
    {
        _logger.LogInformation("Client disconnected with sessionId: {SessionId}", session.Id);
        OnClientDisconnected?.Invoke(session.Id);
        _sessionManager.RemoveSession(session.Id);
        base.OnDisconnected(session);
    }

    protected override void OnStarted()
    {
        _logger.LogInformation("Tcp Server started on {Ip}:{Port}", _darkStarNetworkServerConfig.Address,
            _darkStarNetworkServerConfig.Port);

        base.OnStarted();
    }

    protected override void OnStopped()
    {
        _logger.LogInformation("Network server stopped");
        base.OnStopped();
    }


    public Task SendMessageAsync(Guid sessionId, IDarkStarNetworkMessage message)
    {
        var session = _sessionManager.GetSession(sessionId);

        try
        {
            Sessions[session.SessionId].SendAsync(_messageBuilder.BuildMessage(message));
        }
        catch (Exception ex)

        {
            _logger.LogError("Error during send message to sessionId: {SessionId}: {Error}", sessionId, ex);
        }

        return Task.CompletedTask;
    }

    public Task SendMessageAsync(Guid sessionId, List<IDarkStarNetworkMessage> message)
    {
        var session = _sessionManager.GetSession(sessionId);

        foreach (var messageItem in message)
        {
            try
            {
                var status = Sessions[session.SessionId].SendAsync(_messageBuilder.BuildMessage(messageItem));
            }
            catch (Exception ex)

            {
                _logger.LogError("Error during send message to sessionId: {SessionId}: {Error}", sessionId, ex);
            }
        }

        return Task.CompletedTask;
    }

    public async Task BroadcastMessageAsync(IDarkStarNetworkMessage message)
    {
        foreach (var sessionId in Sessions.Keys)
        {
            await SendMessageAsync(sessionId, message);
        }
    }

    public async Task DispatchMessageReceivedAsync(Guid sessionId, DarkStarMessageType messageType,
        IDarkStarNetworkMessage message)
    {
        OnMessageReceived?.Invoke(sessionId, messageType, message);
        if (_messageListeners.TryGetValue(messageType, out var listener))
        {
            await listener.OnMessageReceivedAsync(sessionId, messageType, message);
        }
    }

    public void RegisterMessageListener(DarkStarMessageType messageType, INetworkServerMessageListener serverMessageListener)
    {
        _messageListeners.Add(messageType, serverMessageListener);
    }

    public Task StartAsync()
    {
        base.Start();

        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        base.Stop();

        return Task.CompletedTask;
    }
}
