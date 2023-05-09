using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using DarkStar.Client.Attributes;
using DarkStar.Client.Models;
using DarkStar.Client.Models.Events;
using DarkStar.Client.PageViews;
using DarkStar.Client.Services;
using DarkStar.Client.ViewModels;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Players;
using DarkStar.Network.Protocol.Types;
using DynamicData;
using ReactiveUI;

namespace DarkStar.Client.PageViewModels;

[PageView(typeof(PlayerSelectPageView))]
public class PlayerSelectPageViewModel : PageViewModelBase
{
    private readonly ServiceContext _serviceContext;
    private readonly TileService _tileService;

    public ObservableCollection<PlayerSelectEntity> Characters { get; set; } = new();

    public ReactiveCommand<Guid, Task> SelectCharacterCommand { get; set; }

    public PlayerSelectPageViewModel(ServiceContext serviceContext, TileService tileService)
    {
        _serviceContext = serviceContext;
        _tileService = tileService;
        _serviceContext.NetworkClient.SubscribeToMessage<PlayerListResponseMessage>(
            DarkStarMessageType.PlayerListResponse,
            OnPlayerListReceived
        );

        _serviceContext.NetworkClient.SubscribeToMessage<PlayerLoginResponseMessage>(DarkStarMessageType.PlayerLoginResponse, OnPlayerLoginResponse);

        _ = Task.Run(
            async () => { await _serviceContext.NetworkClient.SendMessageAsync(new PlayerListRequestMessage()); }
        );

        SelectCharacterCommand = ReactiveCommand.Create<Guid, Task>(
            async (guid) =>
            {
                await _serviceContext.NetworkClient.SendMessageAsync(new PlayerLoginRequestMessage(guid, ""));
            }
        );
    }

    private Task OnPlayerLoginResponse(IDarkStarNetworkMessage arg)
    {
        var message = (PlayerLoginResponseMessage)arg;
        if (message.Success)
        {
            MessageBus.Current.SendMessage(new NavigateToViewEvent(typeof(RenderPageViewModel)));
        }
        return Task.CompletedTask;
    }

    private Task OnPlayerListReceived(IDarkStarNetworkMessage arg)
    {
        return Dispatcher.UIThread.InvokeAsync(
            () =>
            {
                var message = (PlayerListResponseMessage)arg;
                Characters.AddRange(
                    message.Players.Select(
                        s => new PlayerSelectEntity
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Level = s.Level,
                            Image = _tileService.GetTileId((int)s.Tile)
                        }
                    )
                );
            }
        );
    }
}
