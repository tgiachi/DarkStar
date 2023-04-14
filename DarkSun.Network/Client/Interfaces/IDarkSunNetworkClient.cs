using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Interfaces;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;

namespace DarkStar.Network.Client.Interfaces;

public interface IDarkSunNetworkClient
{

    delegate Task MessageReceivedDelegate(DarkStarMessageType messageType, IDarkSunNetworkMessage message);
    event MessageReceivedDelegate OnMessageReceived;

    bool IsConnected { get; }
    ValueTask ConnectAsync();
    ValueTask DisconnectAsync();
    Task SendMessageAsync(IDarkSunNetworkMessage message);
    Task SendMessageAsync(List<IDarkSunNetworkMessage> message);
    void RegisterMessageListener(DarkStarMessageType messageType, INetworkClientMessageListener serverMessageListener);
}
