using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Utils;
using DarkSun.Network.Protocol.Interfaces.Builders;
using DarkSun.Network.Server.Interfaces;
using NetCoreServer;

namespace DarkSun.Network.Server
{
    public class DarkSunTcpSession : TcpSession
    {
        private readonly INetworkMessageBuilder _messageBuilder;
        private readonly IDarkSunNetworkServer _networkServer;
        private bool _isMessageReading;
        private bool _isOrphanedBuffer;
        private int _currentIndex;
        private int _messageLength;
        private Memory<byte> _buffer = new();
        public DarkSunTcpSession(TcpServer server, INetworkMessageBuilder messageBuilder, IDarkSunNetworkServer networkServer) : base(server)
        {
            _messageBuilder = messageBuilder;
            _networkServer = networkServer;
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (!_isMessageReading)
            {
                _isMessageReading = true;
                if (!_isOrphanedBuffer)
                {
                    _messageLength = _messageBuilder.GetMessageLength(buffer);
                    _buffer = new Memory<byte>(buffer, 0, _messageLength);
                }
                else
                {
                    _messageLength = _messageBuilder.GetMessageLength(_buffer.ToArray());
                    _isOrphanedBuffer = false;
                }
            }
            else
            {
                _currentIndex += (int)size;
                if (_currentIndex >= _messageLength)
                {
                    ParseMessage(_buffer[.._messageLength].ToArray());
                    if (_currentIndex - _messageLength > 0)
                    {
                        var orphanedBuffer = _buffer.Slice(_messageLength, _currentIndex - _messageLength);
                        _buffer = new Memory<byte>(orphanedBuffer.ToArray());
                        _isOrphanedBuffer = true;
                    }

                    _isMessageReading = false;
                }
                _buffer = BufferUtils.Combine(_buffer.ToArray(), buffer);
            }

            base.OnReceived(buffer, offset, size);
        }

        private void ParseMessage(Memory<byte> buffer)
        {
            try
            {
                var message = _messageBuilder.ParseMessage(buffer.ToArray());
                _networkServer.DispatchMessageReceived(Id, message.MessageType, message.Message);
            }
            catch (Exception e)
            {
                throw new Exception($"Error during parsing message from sessionId {Id}: {e}");
            }
        }
    }
}
