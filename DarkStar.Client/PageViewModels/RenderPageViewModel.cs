using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Input;
using Avalonia.Threading;
using DarkStar.Api.World.Types.Map;
using DarkStar.Client.Attributes;
using DarkStar.Client.Controls;
using DarkStar.Client.Models;
using DarkStar.Client.Models.Tiles;
using DarkStar.Client.PageViews;
using DarkStar.Client.Services;
using DarkStar.Client.ViewModels;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Map;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Messages.Players;
using DarkStar.Network.Protocol.Messages.Triggers.Npc;
using DarkStar.Network.Protocol.Messages.World;
using DarkStar.Network.Protocol.Types;

using FastEnumUtility;
using ReactiveUI;

namespace DarkStar.Client.PageViewModels;

[PageView(typeof(RenderPageView))]
public class RenderPageViewModel : PageViewModelBase
{
    private readonly GraphicEngineRender _graphicEngineRender;

    public PlayerStatsObject PlayerStats { get; set; } = new();

    private readonly ServiceContext _serviceContext;
    public ReactiveCommand<string, Unit> MoveCharacterCommand { get; set; }

    public ObservableCollection<TextMessageEntity> Messages { get; set; } = new();

    public ReactiveCommand<Unit, Task> PerformActionCommand { get; set; }

    public RenderPageViewModel(GraphicEngineRender graphicEngineRender, ServiceContext serviceContext)
    {
        _graphicEngineRender = graphicEngineRender;
        _serviceContext = serviceContext;

        serviceContext.NetworkClient.SubscribeToMessage<MapResponseMessage>(DarkStarMessageType.MapResponse, OnMapResponse);
        serviceContext.NetworkClient.SubscribeToMessage<PlayerStatsResponseMessage>(
            DarkStarMessageType.PlayerStatsResponse,
            OnPlayerStatReceived
        );
        serviceContext.NetworkClient.SubscribeToMessage<PlayerInventoryResponseMessage>(DarkStarMessageType.PlayerInventoryResponse, OnPlayerInventoryReceived);
        serviceContext.NetworkClient.SubscribeToMessage<WorldMessageResponseMessage>(
            DarkStarMessageType.WorldMessageResponse,
            OnWorldMessage
        );
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

        serviceContext.NetworkClient.SubscribeToMessage<PlayerFovResponseMessage>(
            DarkStarMessageType.PlayerFovResponse,
            OnPlayerFovReceived
        );

        MoveCharacterCommand = ReactiveCommand.Create<string, Unit>(
            s =>
            {
                MoveCharacter(s);
                return Unit.Default;
            }
        );


        PerformActionCommand = ReactiveCommand.Create(PerformAction);
    }

    private Task OnPlayerStatReceived(IDarkStarNetworkMessage arg)
    {

        var message = (PlayerStatsResponseMessage)arg;
        return Dispatcher.UIThread.InvokeAsync(
            () =>
            {
                PlayerStats.Dexterity = message.Dexterity;
                PlayerStats.Experience = message.Experience;
                PlayerStats.Health = message.Health;
                PlayerStats.Intelligence = message.Intelligence;
                PlayerStats.Level = message.Level;
                PlayerStats.Luck = message.Luck;
                PlayerStats.Mana = message.Mana;
                PlayerStats.MaxHealth = message.MaxHealth;
                PlayerStats.MaxMana = message.MaxMana;
                PlayerStats.Strength = message.Strength;
            }
        );
    }

    private Task OnPlayerInventoryReceived(IDarkStarNetworkMessage arg)
    {
        var message = (PlayerInventoryResponseMessage)arg;
        return Dispatcher.UIThread.InvokeAsync(
            () =>
            {


            }
        );
    }

    private Task OnPlayerFovReceived(IDarkStarNetworkMessage arg)
    {
        var message = (PlayerFovResponseMessage)arg;
        _graphicEngineRender.UpdateVisiblePositions(message.VisiblePositions);
        return Task.CompletedTask;
    }

    private Task OnWorldMessage(IDarkStarNetworkMessage arg)
    {
        var message = (WorldMessageResponseMessage)arg;

        return Dispatcher.UIThread.InvokeAsync(
            () =>
            {
                if (Messages.Count >= 50)
                {
                    Messages.Clear();
                }
                Messages.Add(
                    new TextMessageEntity
                    {
                        Name = message.SenderName,
                        Message = message.Message
                    }
                );
            }
        );
    }

    private Task PerformAction()
    {
        //return _serviceContext.NetworkClient.SendMessageAsync()
        return Task.CompletedTask;
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
            () => { _serviceContext.NetworkClient.SendMessageAsync(new PlayerMoveRequestMessage(movement)); }
        );
    }
}
