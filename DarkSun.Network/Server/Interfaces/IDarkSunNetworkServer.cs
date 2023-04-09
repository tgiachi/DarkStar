using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Interfaces;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;

namespace DarkSun.Network.Server.Interfaces
{
    public interface IDarkSunNetworkServer
    {

        delegate Task MessageReceivedDelegate(Guid sessionId, DarkSunMessageType messageType, IDarkSunNetworkMessage message);
        delegate Task<List<IDarkSunNetworkMessage>> ClientConnectedMessages(Guid sessionId);
        delegate Task ClientDisconnectedDelegate(Guid sessionId);

        event MessageReceivedDelegate OnMessageReceived;
        event ClientConnectedMessages OnClientConnected;
        event ClientDisconnectedDelegate OnClientDisconnected;

        Task StartAsync();
        Task StopAsync();
        Task SendMessageAsync(Guid sessionId, IDarkSunNetworkMessage message);
        Task SendMessageAsync(Guid sessionId, List<IDarkSunNetworkMessage> message);
        Task BroadcastMessageAsync(IDarkSunNetworkMessage message);
        Task DispatchMessageReceivedAsync(Guid sessionId, DarkSunMessageType messageType, IDarkSunNetworkMessage message);

        void RegisterMessageListener(DarkSunMessageType messageType, INetworkMessageListener messageListener);
    }
}
