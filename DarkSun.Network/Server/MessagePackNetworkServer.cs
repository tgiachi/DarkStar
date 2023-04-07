using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Data;
using DarkSun.Network.Protocol.Interfaces.Builders;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;
using DarkSun.Network.Server.Interfaces;
using DarkSun.Network.Session.Interfaces;
using Microsoft.Extensions.Logging;
using NetCoreServer;

namespace DarkSun.Network.Server
{
    public class MessagePackNetworkServer : TcpServer, IDarkSunNetworkServer
    {
        private readonly ILogger _logger;
        private readonly INetworkSessionManager _sessionManager;
        private readonly INetworkMessageBuilder _messageBuilder;
        private readonly NetworkServerConfig _networkServerConfig;

        public event IDarkSunNetworkServer.MessageReceivedDelegate? OnMessageReceived;
        public event IDarkSunNetworkServer.ClientConnectedMessages? OnClientConnected;
        public event IDarkSunNetworkServer.ClientDisconnectedDelegate? OnClientDisconnected;

        public MessagePackNetworkServer(ILogger<MessagePackNetworkServer> logger,
            INetworkSessionManager sessionManager,
            INetworkMessageBuilder messageBuilder,
            NetworkServerConfig networkServerConfig) : base(networkServerConfig.Address, networkServerConfig.Port)
        {
            _logger = logger;
            _sessionManager = sessionManager;
            _messageBuilder = messageBuilder;
            _networkServerConfig = networkServerConfig;
        }

        protected override TcpSession CreateSession()
        {
            return new DarkSunTcpSession(this, _messageBuilder, this);
        }

        protected override void OnConnected(TcpSession session)
        {
            _logger.LogInformation("Client {IpAddress} connected with sessionId: {SessionId}", session.Socket.RemoteEndPoint, session.Id);

            _sessionManager.AddSession(session.Id);
            var messagesToSend = OnClientConnected?.Invoke(session.Id).GetAwaiter().GetResult();
            if (messagesToSend != null)
            {
                SendMessageAsync(session.Id, messagesToSend);
            }

            base.OnConnected(session);
        }

        protected override void OnDisconnected(TcpSession session)
        {
            _logger.LogInformation("Client {IpAddress} disconnected with sessionId: {SessionId}", session.Socket.RemoteEndPoint, session.Id);
            OnClientDisconnected?.Invoke(session.Id);
            _sessionManager.RemoveSession(session.Id);
            base.OnDisconnected(session);
        }

        protected override void OnStarted()
        {
            _logger.LogInformation("Server started on {Ip}:{Port}", _networkServerConfig.Address, _networkServerConfig.Port);

            base.OnStarted();
        }

        protected override void OnStopped()
        {
            _logger.LogInformation("Network server stopped");
            base.OnStopped();
        }



        public Task SendMessageAsync(Guid sessionId, IDarkSunNetworkMessage message)
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

        public Task SendMessageAsync(Guid sessionId, List<IDarkSunNetworkMessage> message)
        {
            var session = _sessionManager.GetSession(sessionId);

            foreach (var messageItem in message)
            {
                try
                {
                    Sessions[session.SessionId].SendAsync(_messageBuilder.BuildMessage(messageItem));
                }
                catch (Exception ex)

                {
                    _logger.LogError("Error during send message to sessionId: {SessionId}: {Error}", sessionId, ex);
                }
            }
            return Task.CompletedTask;
        }

        public async Task BroadcastMessageAsync(IDarkSunNetworkMessage message)
        {
            foreach (var sessionId in Sessions.Keys)
            {
                await SendMessageAsync(sessionId, message);
            }
        }

        public Task DispatchMessageReceived(Guid sessionId, DarkSunMessageType messageType, IDarkSunNetworkMessage message)
        {
            OnMessageReceived?.Invoke(sessionId, messageType, message);
            return Task.CompletedTask;
        }



        public new Task Start()
        {
            base.Start();

            return Task.CompletedTask;
        }

        public new Task Stop()
        {
            base.Stop();

            return Task.CompletedTask;
        }
    }
}
