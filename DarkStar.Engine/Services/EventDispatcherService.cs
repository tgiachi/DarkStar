using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Engine.Events.Engine;
using DarkStar.Api.Engine.Events.Map;
using DarkStar.Api.Engine.Events.Players;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.World.Types.Map;
using DarkStar.Database.Entities.Npc;
using DarkStar.Database.Entities.Objects;
using DarkStar.Database.Entities.Player;
using DarkStar.Engine.MessageListeners.Helpers;
using DarkStar.Engine.Services.Base;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Live;
using DarkStar.Network.Protocol.Messages.Players;
using DarkStar.Network.Protocol.Messages.Triggers.Npc;
using DarkStar.Network.Protocol.Messages.Triggers.WorldObject;
using Microsoft.Extensions.Logging;


namespace DarkStar.Engine.Services;

[DarkStarEngineService(nameof(EventDispatcherService), 1000)]
public class EventDispatcherService : BaseService<EventDispatcherService>, IEventDispatcherService
{
    private readonly List<Action> _engineReadyActions = new();

    public EventDispatcherService(ILogger<EventDispatcherService> logger) : base(logger)
    {
    }

    protected override ValueTask<bool> StartAsync()
    {
        Logger.LogDebug("Subscribing events");
        SubscribeToEvent<GameObjectAddedEvent>(OnGameObjectAdded);
        SubscribeToEvent<GameObjectMovedEvent>(OnGameObjectMoved);
        SubscribeToEvent<GameObjectRemovedEvent>(OnGameObjectRemoved);
        SubscribeToEvent<PingRequestEvent>(OnPingRequestMessage);
        SubscribeToEvent<PlayerLoggedEvent>(OnPlayerLoggedMessage);
        SubscribeToEvent<EngineReadyEvent>(OnEngineReadyMessage);
        return ValueTask.FromResult(true);
    }

    private void OnEngineReadyMessage(EngineReadyEvent obj)
    {
        _ = Task.Run(
            () =>
            {
                foreach (var action in _engineReadyActions)
                {
                    action();
                }
            }
        );
    }

    /// <summary>
    /// This event is triggered when a player login in the world
    /// </summary>
    /// <param name="obj"></param>
    private void OnPlayerLoggedMessage(PlayerLoggedEvent obj)
    {
        _ = Task.Run(
            async () =>
            {
                await Engine.NetworkServer.SendMessageAsync(obj.SessionId, new PlayerLoginResponseMessage(true));

                await Task.Delay(500);
                await Engine.NetworkServer.SendMessageAsync(
                    obj.SessionId,
                    await PlayerDataHelper.BuildPlayerStatsAsync(
                        Engine,
                        obj.PlayerId
                    )
                );
                Logger.LogDebug("Sending information of map to player {PlayerId}", obj.PlayerId);
                Engine.NetworkServer.SendMessageAsync(
                    obj.SessionId,
                    await MapDataHelper.BuildMapResponseDataAsync(Engine, obj.MapId, obj.PlayerId)
                );

                await Task.Delay(1000);
                var fov = await Engine.WorldService.GetFovAsync(obj.MapId, obj.Position);
                Engine.NetworkServer.SendMessageAsync(obj.SessionId, new PlayerFovResponseMessage(fov));

                Engine.NetworkServer.SendMessageAsync(
                    obj.SessionId,
                    await PlayerDataHelper.BuildPlayerDataResponse(Engine, obj.PlayerId)
                );

                Engine.NetworkServer.SendMessageAsync(
                    obj.SessionId,
                    await PlayerDataHelper.BuildPlayerInventoryAsync(Engine, obj.PlayerId)
                );
            }
        );
    }

    private async void OnGameObjectAdded(GameObjectAddedEvent obj)
    {
        var playerToNotify = Engine.WorldService.GetPlayers(obj.MapId);
        var message = await HandleEntityAddedAsync(obj);

        if (message != null)
        {
            foreach (var player in playerToNotify)
            {
                await Engine.NetworkServer.SendMessageAsync(player.NetworkSessionId, message);
            }
        }
    }


    private async void OnGameObjectRemoved(GameObjectRemovedEvent obj)
    {
        var playerToNotify = Engine.WorldService.GetPlayers(obj.MapId);

        var message = await HandleEntityRemovedAsync(obj);
        foreach (var player in playerToNotify)
        {
            Engine.NetworkServer.SendMessageAsync(player.NetworkSessionId, message);
        }
    }


    private async void OnGameObjectMoved(GameObjectMovedEvent obj)
    {
        var playerToNotify = Engine.WorldService.GetPlayers(obj.MapId);
        var message = await HandleEntityMovedAsync(obj);
        if (message != null)
        {
            foreach (var player in playerToNotify)
            {
                Engine.NetworkServer.SendMessageAsync(player.NetworkSessionId, message);
            }
        }
    }

    private async Task<IDarkStarNetworkMessage?> HandleEntityAddedAsync(GameObjectAddedEvent @event)
    {
        IDarkStarNetworkMessage message = null!;
        switch (@event.Layer)
        {
            case MapLayer.Creatures:
                var npc = await Engine.DatabaseService.QueryAsSingleAsync<NpcEntity>(entity => entity.Id == @entity.Id);
                message = new NpcAddedResponseMessage(
                    @event.MapId,
                    @event.Id.ToString(),
                    npc.Name,
                    @event.Position,
                    (int)npc.TileId
                );
                break;
            case MapLayer.Objects:
                var gameObject =
                    await Engine.DatabaseService.QueryAsSingleAsync<GameObjectEntity>(entity => entity.Id == @entity.Id);
                message = new WorldObjectAddedResponseMessage(
                    @event.MapId,
                    @event.Id.ToString(),
                    gameObject.Name,
                    @event.Position,
                    (int)gameObject.TileId
                );
                break;
            case MapLayer.Players:
                var player = await Engine.DatabaseService.QueryAsSingleAsync<PlayerEntity>(
                    entity => entity.Id == @entity.Id
                );
                message = new PlayerGameObjectAddedResponseMessage(
                    @event.MapId,
                    player.Name,
                    player.Id.ToString(),
                    @event.Position,
                    (int)player.TileId
                );
                break;
        }

        return message;
    }

    private async Task<IDarkStarNetworkMessage?> HandleEntityMovedAsync(GameObjectMovedEvent @event)
    {
        IDarkStarNetworkMessage message = @event.Layer switch
        {
            MapLayer.Creatures => new NpcMovedResponseMessage(@event.MapId, @event.Id.ToString(), @event.Position),
            MapLayer.Objects   => new WorldObjectMovedResponseMessage(@event.MapId, @event.Id.ToString(), @event.Position),
            MapLayer.Players => new PlayerGameObjectMovedResponseMessage(
                @event.MapId,
                @event.Id.ToString(),
                @event.Position
            ),
            _ => null!
        };

        return message;
    }

    private async Task<IDarkStarNetworkMessage?> HandleEntityRemovedAsync(GameObjectRemovedEvent @event)
    {
        IDarkStarNetworkMessage message = @event.Layer switch
        {
            MapLayer.Players   => new PlayerGameObjectRemovedResponseMessage(@event.MapId, @event.Id.ToString()),
            MapLayer.Creatures => new NpcRemovedResponseMessage(@event.MapId, @event.Id.ToString()),
            MapLayer.Objects   => new WorldObjectRemovedResponseMessage(@event.MapId, @event.Id.ToString()),
            _                  => null!
        };

        return message;
    }

    private void OnPingRequestMessage(PingRequestEvent obj)
    {
        _ = Task.Run(async () => await Engine.NetworkServer.BroadcastMessageAsync(new PingMessageResponse()));
    }

    public void AddEngineReadyEvent(Action callback)
    {
        _engineReadyActions.Add(callback);
    }
}
