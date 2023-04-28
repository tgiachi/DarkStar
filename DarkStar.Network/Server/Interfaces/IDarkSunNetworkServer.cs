using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Interfaces;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;

namespace DarkStar.Network.Server.Interfaces;

public interface IDarkSunNetworkServer
{
    delegate Task MessageReceivedDelegate(string sessionId, DarkStarMessageType messageType,
        IDarkStarNetworkMessage message);

    delegate Task<List<IDarkStarNetworkMessage>> ClientConnectedMessages(string sessionId);

    delegate Task ClientDisconnectedDelegate(string sessionId);

    event MessageReceivedDelegate OnMessageReceived;
    event ClientConnectedMessages OnClientConnected;
    event ClientDisconnectedDelegate OnClientDisconnected;

    Task StartAsync();
    Task StopAsync();
    Task SendMessageAsync(string sessionId, IDarkStarNetworkMessage message);
    Task SendMessageAsync(string sessionId, List<IDarkStarNetworkMessage> message);
    Task BroadcastMessageAsync(IDarkStarNetworkMessage message);
    Task DispatchMessageReceivedAsync(string sessionId, DarkStarMessageType messageType, IDarkStarNetworkMessage message);
    void RegisterMessageListener(DarkStarMessageType messageType, INetworkServerMessageListener serverMessageListener);
}
