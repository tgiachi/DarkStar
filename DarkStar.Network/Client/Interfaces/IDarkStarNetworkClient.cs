using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Data;
using DarkStar.Network.Interfaces;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;

namespace DarkStar.Network.Client.Interfaces;

public interface IDarkStarNetworkClient
{
    delegate Task MessageReceivedDelegate(DarkStarMessageType messageType, IDarkStarNetworkMessage message);



    delegate Task ClientConnectedDelegate();

    event  ClientConnectedDelegate OnClientConnected;

    event MessageReceivedDelegate OnMessageReceived;

    bool IsConnected { get; }
    ValueTask ConnectAsync();

    ValueTask ConnectAsync(DarkStarNetworkClientConfig config);

    ValueTask DisconnectAsync();
    Task SendMessageAsync(IDarkStarNetworkMessage message);
    Task SendMessageAsync(List<IDarkStarNetworkMessage> message);
    void SubscribeToMessage<TMessage>(DarkStarMessageType messageType, Func<IDarkStarNetworkMessage, Task > handler) where TMessage : IDarkStarNetworkMessage;

    void RegisterMessageListener(DarkStarMessageType messageType, INetworkClientMessageListener serverMessageListener);

    void UnregisterMessageListener(DarkStarMessageType messageType, INetworkClientMessageListener clientMessageListener);


}
