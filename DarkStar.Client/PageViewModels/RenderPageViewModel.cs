using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Input;
using DarkStar.Api.World.Types.Map;
using DarkStar.Client.Attributes;
using DarkStar.Client.Controls;
using DarkStar.Client.Models.Tiles;
using DarkStar.Client.PageViews;
using DarkStar.Client.Services;
using DarkStar.Client.ViewModels;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Map;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Messages.Players;
using DarkStar.Network.Protocol.Messages.Triggers.Npc;
using DarkStar.Network.Protocol.Types;
using FastEnumUtility;
using ReactiveUI;

namespace DarkStar.Client.PageViewModels;

[PageView(typeof(RenderPageView))]
public class RenderPageViewModel : PageViewModelBase
{
    private readonly GraphicEngineRender _graphicEngineRender;

    private readonly ServiceContext _serviceContext;
    public ReactiveCommand<string, Unit> MoveCharacterCommand { get; set; }

    public RenderPageViewModel(GraphicEngineRender graphicEngineRender, ServiceContext serviceContext)
    {
        _graphicEngineRender = graphicEngineRender;
        _serviceContext = serviceContext;

        serviceContext.NetworkClient.SubscribeToMessage<MapResponseMessage>(DarkStarMessageType.MapResponse, OnMapResponse);
        serviceContext.NetworkClient.SubscribeToMessage<PlayerMoveResponseMessage>(
            DarkStarMessageType.PlayerMoveResponse,
            OnPlayerMoved
        );
        serviceContext.NetworkClient.SubscribeToMessage<PlayerDataResponseMessage>(
            DarkStarMessageType.PlayerDataResponse,
            OnPlayerData
        );
        serviceContext.NetworkClient.SubscribeToMessage<NpcMovedResponseMessage>(
            DarkStarMessageType.NpcMovedResponse,
            OnNpcMoved
        );

        MoveCharacterCommand = ReactiveCommand.Create<string, Unit>(
            s =>
            {
                MoveCharacter(s);
                return Unit.Default;
            });

    }

    private Task OnPlayerMoved(IDarkStarNetworkMessage arg)
    {
        var message = (PlayerMoveResponseMessage)arg;

        _graphicEngineRender.MoveTile(MapLayer.Players, message.PlayerId, message.Position);

        return Task.CompletedTask;
    }

    private Task OnPlayerData(IDarkStarNetworkMessage arg)
    {
        var message = (PlayerDataResponseMessage)arg;
        _graphicEngineRender.AddPlayer(message.PlayerId.ToString(), message.TileId, message.Position);

        return Task.CompletedTask;
    }

    private Task OnNpcMoved(IDarkStarNetworkMessage arg)
    {
        var message = (NpcMovedResponseMessage)arg;

        _graphicEngineRender.MoveTile(MapLayer.Creatures, message.NpcId, message.Position);

        return Task.CompletedTask;
    }

    private Task OnMapResponse(IDarkStarNetworkMessage arg)
    {
        var message = (MapResponseMessage)arg;

        _graphicEngineRender.ClearLayers();

        foreach (var terrain in message.TerrainsLayer)
        {
            _graphicEngineRender.AddTile(
                MapLayer.Terrain,
                new Tile(terrain.Id.ToString(), terrain.TileType, terrain.Position)
            );
        }

        foreach (var npc in message.NpcsLayer)
        {
            _graphicEngineRender.AddTile(MapLayer.Creatures, new Tile(npc.Id.ToString(), npc.TileType, npc.Position));
        }

        foreach (var gameObject in message.GameObjectsLayer)
        {
            _graphicEngineRender.AddTile(
                MapLayer.Objects,
                new Tile(gameObject.Id.ToString(), gameObject.TileType, gameObject.Position)
            );
        }

        foreach (var item in message.ItemsLayer)
        {
            _graphicEngineRender.AddTile(
                MapLayer.Items,
                new Tile(item.Id.ToString(), item.TileType, item.Position)
            );
        }

        return Task.CompletedTask;
    }

    public void SetupRenderControl(RenderControl renderControl)
    {
        renderControl.RenderAction = _graphicEngineRender.RenderAction;
    }

    public void DispatchKey(Key key, KeyModifiers modifiers)
    {
    }

    public void DispatchMouseMove(Point point)
    {
    }

    public void DispatchMouseWheel(Vector delta)
    {
    }

    public void MoveCharacter(string direction)
    {
        var movement = FastEnum.Parse<MoveDirectionType>(direction);
        _ = Task.Run(
            () =>
            {
                _serviceContext.NetworkClient.SendMessageAsync(new PlayerMoveRequestMessage(movement));
            }
        );

    }


}
