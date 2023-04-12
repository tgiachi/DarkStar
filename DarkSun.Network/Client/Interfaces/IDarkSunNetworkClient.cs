using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Interfaces;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;

namespace DarkSun.Network.Client.Interfaces;

public interface IDarkSunNetworkClient
{

    delegate Task MessageReceivedDelegate(DarkSunMessageType messageType, IDarkSunNetworkMessage message);
    event MessageReceivedDelegate OnMessageReceived;

    bool IsConnected { get; }
    ValueTask ConnectAsync();
    ValueTask DisconnectAsync();

    Task SendMessageAsync(IDarkSunNetworkMessage message);
    Task SendMessageAsync(List<IDarkSunNetworkMessage> message);

    void RegisterMessageListener(DarkSunMessageType messageType, INetworkClientMessageListener serverMessageListener);
}
