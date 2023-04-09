using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

        private int _currentIndex;

        private readonly byte[] _separators;
        private int _tokenIndex = 0;
        private readonly int _bufferChunk = 1024;
        private readonly byte[] _tempBuffer = new byte[1];
        private byte[] _buffer = Array.Empty<byte>();

        public DarkSunTcpSession(TcpServer server, INetworkMessageBuilder messageBuilder, IDarkSunNetworkServer networkServer) : base(server)
        {
            _messageBuilder = messageBuilder;
            _networkServer = networkServer;
            _separators = messageBuilder.GetMessageSeparators;
            _currentIndex = 0;

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

        private void ParseMessage(Memory<byte> buffer)
        {
            try
            {
                var message = _messageBuilder.ParseMessage(buffer.ToArray());
                _networkServer.DispatchMessageReceivedAsync(Id, message.MessageType, message.Message);
            }
            catch (Exception e)
            {
                throw new Exception($"Error during parsing message from sessionId {Id}: {e}");
            }
        }
    }
}
