using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Utils;
using DarkSun.Network.Client.Interfaces;
using DarkSun.Network.Data;
using DarkSun.Network.Interfaces;
using DarkSun.Network.Protocol.Interfaces.Builders;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;
using Microsoft.Extensions.Logging;
using TcpClient = NetCoreServer.TcpClient;

namespace DarkSun.Network.Client
{
    public class TcpNetworkClient : TcpClient, IDarkSunNetworkClient
    {
        public event IDarkSunNetworkClient.MessageReceivedDelegate? OnMessageReceived;

        private readonly ILogger _logger;
        private readonly INetworkMessageBuilder _messageBuilder;
        private readonly Dictionary<DarkSunMessageType, INetworkClientMessageListener> _messageListeners = new();

        private int _currentIndex;

        private readonly byte[] _separators;
        private int _tokenIndex = 0;
        private readonly int _bufferChunk = 1024;
        private readonly byte[] _tempBuffer = new byte[1];
        private byte[] _buffer = Array.Empty<byte>();

        public TcpNetworkClient(ILogger<TcpNetworkClient> logger, 
            DarkSunNetworkClientConfig config, 
            INetworkMessageBuilder messageBuilder) : base(config.Address, config.Port)
        {
            _logger = logger;
            _messageBuilder = messageBuilder;
         //   OptionReceiveBufferSize = 512;
            _separators = messageBuilder.GetMessageSeparators;
            _currentIndex = 0;
          
            
        }

        protected override void OnConnected()
        {
            _logger.LogInformation("Connected to {IpAddress}", Socket.RemoteEndPoint);
            base.OnConnected();
        }

        protected override void OnDisconnected()
        {
            _logger.LogInformation("Disconnected from server");
            base.OnDisconnected();
        }

        protected override void OnError(SocketError error)
        {
            base.OnError(error);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (_currentIndex + size >= _buffer.Length)
            {
                _buffer = BufferUtils.Combine(_buffer, new byte[_bufferChunk]);
            }

            for (var i = 0; i < size; i++)
            {
                if (_currentIndex + size >= _buffer.Length)
                {
                    _buffer = BufferUtils.Combine(_buffer, new byte[_bufferChunk]);
                }

                _buffer[_currentIndex] = buffer[i];
                _tempBuffer[0] = buffer[i];
                _currentIndex++;

                if (_tempBuffer[0] == _separators[_tokenIndex])
                {
                    _tokenIndex++;

                    if (_tokenIndex != _separators.Length)
                    {
                        continue;
                    }

                    ParseMessage(_buffer[.._currentIndex]);
                    _buffer = new byte[_bufferChunk];
                    _currentIndex = 0;

                    _tokenIndex = 0;
                }
                else
                {
                    _tokenIndex = 0;
                }
            }
            base.OnReceived(buffer, offset, size);
        }

        public async Task DispatchMessageReceivedAsync( DarkSunMessageType messageType,
            IDarkSunNetworkMessage message)
        {
            _logger.LogDebug("Received message: {MessageType}", messageType);
            OnMessageReceived?.Invoke(messageType, message);
            
            if (_messageListeners.TryGetValue(messageType, out var listener))
            {
                await listener.OnMessageReceivedAsync( messageType, message);
            }
        }

        private async void ParseMessage(Memory<byte> buffer)
        {
            try
            {
                var message = _messageBuilder.ParseMessage(buffer.ToArray());
                await DispatchMessageReceivedAsync(message.MessageType, message.Message);

            }
            catch (Exception e)
            {
                throw new Exception($"Error during parsing message from sessionId {e}");
            }
        }

        public Task SendMessageAsync(IDarkSunNetworkMessage message)
        {
            try
            {
                SendAsync(_messageBuilder.BuildMessage(message));
 
            }
            catch (Exception ex)

            {
                _logger.LogError("Error during send message to: {Error}", ex);
            }

            return Task.CompletedTask;
        }

        public Task SendMessageAsync(List<IDarkSunNetworkMessage> message)
        {
            
            foreach (var messageItem in message)
            {
                try
                {
                    SendAsync(_messageBuilder.BuildMessage(messageItem));
                }
                catch (Exception ex)

                {
                    _logger.LogError("Error during send message to sessionId: {Error}", ex);
                }
            }

            return Task.CompletedTask;
        }

        public void RegisterMessageListener(DarkSunMessageType messageType, INetworkClientMessageListener serverMessageListener)
        {
            _messageListeners.Add(messageType, serverMessageListener);
        }

      

        public new ValueTask ConnectAsync()
        {
            base.ConnectAsync();

            return ValueTask.CompletedTask;
        }

        public new ValueTask DisconnectAsync()
        {
            if (IsConnected)
            {
                base.Disconnect();
            }

            return ValueTask.CompletedTask;
        }
    }
}
