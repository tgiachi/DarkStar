using DarkStar.Network.Client.Interfaces;
using DarkStar.Network.Interfaces;
using DarkStar.Network.Protocol.Interfaces.Builders;
using DarkStar.Network.Protocol.Types;
using Microsoft.Extensions.Logging;
using System.Text;
using DarkStar.Network.Data;
using DarkStar.Network.Protocol.Interfaces.Messages;
using Humanizer;
using Microsoft.AspNetCore.SignalR.Client;

namespace DarkStar.Network.Client;

public class SignalrNetworkClient : IDarkStarNetworkClient
{
    public event IDarkStarNetworkClient.ClientConnectedDelegate? OnClientConnected;
    public event IDarkStarNetworkClient.MessageReceivedDelegate? OnMessageReceived;

    private readonly ILogger _logger;
    private readonly INetworkMessageBuilder _messageBuilder;
    private DarkStarNetworkClientConfig _clientConfig;
    private HubConnection _hubConnection;
    private readonly Dictionary<DarkStarMessageType, INetworkClientMessageListener> _messageListeners = new();
    private readonly Dictionary<DarkStarMessageType, Func<IDarkStarNetworkMessage, Task>> _actionMessageListeners = new();

    public SignalrNetworkClient(
        ILogger<SignalrNetworkClient> logger, INetworkMessageBuilder messageBuilder, DarkStarNetworkClientConfig clientConfig
    )
    {
        _logger = logger;
        _messageBuilder = messageBuilder;
        _clientConfig = clientConfig;
    }

    public bool IsConnected { get; private set; }

    public async ValueTask ConnectAsync()
    {
        _hubConnection = new HubConnectionBuilder().WithUrl($"{_clientConfig.Address}:{_clientConfig.Port}/messages")
            .Build();
        _hubConnection.Closed += async (error) =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await _hubConnection.StartAsync();
        };

        _hubConnection.On<string>("IncomingMessage", async (message) => { await OnMessageReceivedAsync(message); });
        await _hubConnection.StartAsync();
    }

    public ValueTask ConnectAsync(DarkStarNetworkClientConfig config)
    {
        _clientConfig = config;

        return ConnectAsync();
    }

    private async Task OnMessageReceivedAsync(string message)
    {
        try
        {
            if (!IsConnected)
            {
                OnClientConnected?.Invoke();
                IsConnected = true;
            }

            var parsedMessage = _messageBuilder.ParseMessage(Encoding.UTF8.GetBytes(message));
            _logger.LogDebug(
                "Received message: {MessageType} size: {Size}",
                parsedMessage.MessageType,
                message.Length.Bytes()
            );
            await DispatchMessageReceivedAsync(parsedMessage.MessageType, parsedMessage.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while processing message: {Message}", message);
        }
    }

    public ValueTask DisconnectAsync() => _hubConnection.DisposeAsync();

    public Task SendMessageAsync(IDarkStarNetworkMessage message)
    {
        var msg = _messageBuilder.BuildMessage(message);
        return _hubConnection.SendAsync("SendMessage", Encoding.UTF8.GetString(msg));
    }

    public async Task SendMessageAsync(List<IDarkStarNetworkMessage> messages)
    {
        foreach (var message in messages)
        {
            await SendMessageAsync(message);
        }
    }

    public void SubscribeToMessage<TMessage>(DarkStarMessageType messageType, Func<IDarkStarNetworkMessage, Task> handler)
        where TMessage : IDarkStarNetworkMessage
    {
        _actionMessageListeners.Add(messageType, handler);
    }

    public void RegisterMessageListener(DarkStarMessageType messageType, INetworkClientMessageListener serverMessageListener)
    {
        _messageListeners.Add(messageType, serverMessageListener);
    }

    public void UnregisterMessageListener(
        DarkStarMessageType messageType, INetworkClientMessageListener clientMessageListener
    )
    {
        if (_messageListeners.ContainsKey(messageType))
        {
            _messageListeners.Remove(messageType);
        }
    }

    public async Task DispatchMessageReceivedAsync(
        DarkStarMessageType messageType,
        IDarkStarNetworkMessage message
    )
    {
        _logger.LogDebug("Received message: {MessageType}", messageType);
        OnMessageReceived?.Invoke(messageType, message);

        if (_messageListeners.TryGetValue(messageType, out var listener))
        {
            await listener.OnMessageReceivedAsync(messageType, message);
        }

        if (_actionMessageListeners.TryGetValue(messageType, out var actionListener))
        {
            await actionListener.Invoke(message);
        }
    }
}
