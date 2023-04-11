using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Protocol.Interfaces.Messages;

namespace DarkSun.Api.Engine.Interfaces.Listener
{
    public interface INetworkConnectionHandler
    {
        Task<List<IDarkSunNetworkMessage>> ClientConnectedMessagesAsync(Guid sessionId);
        Task ClientDisconnectedAsync(Guid sessionId);
    }
}
