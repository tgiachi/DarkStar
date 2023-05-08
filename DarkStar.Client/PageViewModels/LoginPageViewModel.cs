using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Threading;
using DarkStar.Client.Attributes;
using DarkStar.Client.PageViews;
using DarkStar.Client.Services;
using DarkStar.Client.ViewModels;
using DarkStar.Network.Data;
using DarkStar.Network.Interfaces;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Server;
using DarkStar.Network.Protocol.Types;
using ReactiveUI;

namespace DarkStar.Client.PageViewModels;

[PageView(typeof(LoginPageView))]
public class LoginPageViewModel : PageViewModelBase, INetworkClientMessageListener
{
    private readonly ServiceContext _serviceContext;
    public ObservableCollection<string> Servers { get; set; }

    public string SelectedServer { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public string ServerName { get; set; }
    public string ServerVersion { get; set; }

    public ReactiveCommand<Unit, Task> LoginCommand { get; set; }

    public LoginPageViewModel(ServiceContext serviceContext)
    {
        _serviceContext = serviceContext;
        _serviceContext.NetworkClient.RegisterMessageListener(DarkStarMessageType.ServerVersionResponse, this);
        _serviceContext.NetworkClient.RegisterMessageListener(DarkStarMessageType.ServerNameResponse, this);

        Servers = new ObservableCollection<string> { "http://localhost:5000/" };

        Username = "";
        Password = "";

        LoginCommand = ReactiveCommand.Create(
            async () =>
            {
                var parsedUri = new Uri(SelectedServer);
                await _serviceContext.NetworkClient.ConnectAsync(
                    new DarkStarNetworkClientConfig
                    {
                        Address = $"http://{parsedUri.Host}",
                        Port = parsedUri.Port
                    }
                );
            }
        );
    }

    public override Task OnClose()
    {
        _serviceContext.NetworkClient.UnregisterMessageListener(DarkStarMessageType.ServerVersionResponse, this);
        return Task.CompletedTask;
    }

    public Task OnMessageReceivedAsync(DarkStarMessageType messageType, IDarkStarNetworkMessage message)
    {
        return Dispatcher.UIThread.InvokeAsync(
            () =>
            {
                if (messageType == DarkStarMessageType.ServerVersionResponse)
                {
                    var serverVersionResponse = (ServerVersionResponseMessage)message;
                    ServerVersion =
                        $"v {serverVersionResponse.Major}.{serverVersionResponse.Minor}.{serverVersionResponse.Build}";
                }

                if (messageType == DarkStarMessageType.ServerNameResponse)
                {
                    var serverNameResponse = (ServerNameResponseMessage)message;
                    ServerName = serverNameResponse.ServerName;
                }
            }
        );
    }
}
