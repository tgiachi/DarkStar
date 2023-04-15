using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Protocol.Interfaces.Messages;

namespace DarkStar.Api.Engine.Interfaces.Listener;

public interface INetworkConnectionHandler
{
    Task<List<IDarkSunNetworkMessage>> ClientConnectedMessagesAsync(Guid sessionId);
    Task ClientDisconnectedAsync(Guid sessionId);
}
