using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Threading;
using DarkStar.Client.Attributes;
using DarkStar.Client.Models.Events;
using DarkStar.Client.PageViews;
using DarkStar.Client.Services;
using DarkStar.Client.ViewModels;
using DarkStar.Network.Data;
using DarkStar.Network.Interfaces;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Accounts;
using DarkStar.Network.Protocol.Messages.Server;
using DarkStar.Network.Protocol.Messages.TileSet;
using DarkStar.Network.Protocol.Types;
using ReactiveUI;

namespace DarkStar.Client.PageViewModels;

[PageView(typeof(LoginPageView))]
public class LoginPageViewModel : PageViewModelBase, INetworkClientMessageListener
{
    private readonly ServiceContext _serviceContext;
    private readonly TileService _tileService;
    public ObservableCollection<string> Servers { get; set; }

    public string SelectedServer { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public string ErrorConnection { get; set; }


    public ReactiveCommand<Unit, Task> LoginCommand { get; set; }

    public LoginPageViewModel(ServiceContext serviceContext, TileService tileService)
    {
        _serviceContext = serviceContext;
        _serviceContext.NetworkClient.OnClientConnected += NetworkClientOnOnClientConnected;
        _serviceContext.NetworkClient.RegisterMessageListener(DarkStarMessageType.ServerVersionResponse, this);
        _serviceContext.NetworkClient.RegisterMessageListener(DarkStarMessageType.ServerNameResponse, this);
        _serviceContext.NetworkClient.RegisterMessageListener(DarkStarMessageType.AccountLoginResponse, this);
        _tileService = tileService;
        Servers = new ObservableCollection<string> { "http://localhost:5000/" };

        Username = "";
        Password = "";

        LoginCommand = ReactiveCommand.Create(
            async () =>
            {
                var parsedUri = new Uri(SelectedServer);
                _serviceContext.ServerUrl = parsedUri.ToString();
                await _serviceContext.NetworkClient.ConnectAsync(
                    new DarkStarNetworkClientConfig
                    {
                        Address = $"http://localhost",
                        Port = 5000
                    }
                );
            }
        );
    }

    private async Task NetworkClientOnOnClientConnected()
    {
        MessageBus.Current.SendMessage(new OnConnectedEvent());
        await _serviceContext.NetworkClient.SendMessageAsync(new AccountLoginRequestMessage(Username, Password));
    }

    public override Task OnClose()
    {
        _serviceContext.NetworkClient.UnregisterMessageListener(DarkStarMessageType.ServerVersionResponse, this);
        _serviceContext.NetworkClient.UnregisterMessageListener(DarkStarMessageType.ServerNameResponse, this);
        _serviceContext.NetworkClient.UnregisterMessageListener(DarkStarMessageType.AccountLoginResponse, this);
        _serviceContext.NetworkClient.OnClientConnected -= NetworkClientOnOnClientConnected;
        return Task.CompletedTask;
    }

    public Task OnMessageReceivedAsync(DarkStarMessageType messageType, IDarkStarNetworkMessage message)
    {
        return Dispatcher.UIThread.InvokeAsync(
            async () =>
            {
                if (messageType == DarkStarMessageType.ServerVersionResponse)
                {
                    var serverVersionResponse = (ServerVersionResponseMessage)message;
                    MessageBus.Current.SendMessage(
                        new ServerVersionEvent()
                        {
                            ServerVersion =
                                $"{serverVersionResponse.Major}.{serverVersionResponse.Minor}.{serverVersionResponse.Build}"
                        }
                    );
                }

                if (messageType == DarkStarMessageType.ServerNameResponse)
                {
                    var serverNameResponse = (ServerNameResponseMessage)message;
                    MessageBus.Current.SendMessage(
                        new ServerNameEvent()
                        {
                            Name = serverNameResponse.ServerName
                        }
                    );
                }


                if (messageType == DarkStarMessageType.AccountLoginResponse)
                {
                    var accountLoginResponse = (AccountLoginResponseMessage)message;
                    if (!accountLoginResponse.Success)
                    {
                        ErrorConnection = "Invalid username or password!";
                    }
                    else
                    {

                        while (!_tileService.TilesReady)
                        {
                            await Task.Delay(1000);
                        }

                        MessageBus.Current.SendMessage(new NavigateToViewEvent(typeof(PlayerSelectPageViewModel)));
                    }
                }
            }
        );
    }
}
