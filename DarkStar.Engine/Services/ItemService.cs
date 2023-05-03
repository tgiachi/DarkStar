using System.Reflection;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Engine.Attributes.Objects;
using DarkStar.Api.Engine.Data.Items;
using DarkStar.Api.Engine.Events.Map;
using DarkStar.Api.Engine.Interfaces.Objects;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.Items.WorldObjects.Base;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.Utils;
using DarkStar.Api.World.Types.GameObjects;
using DarkStar.Api.World.Types.Map;
using DarkStar.Database.Entities.Objects;
using DarkStar.Engine.Services.Base;
using DarkStar.Network.Protocol.Messages.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Services;

[DarkStarEngineService(nameof(ItemService), 6)]
public class ItemService : BaseService<ItemService>, IItemService
{
    private readonly SemaphoreSlim _gameObjectActionLock = new(1);
    private readonly ITypeService _typeService;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, Type> _gameObjectActionTypes = new();
    private readonly Dictionary<uint, IGameObjectAction> _gameObjectActions = new();
    private readonly Dictionary<uint, IScheduledGameObjectAction> _scheduledGameObjectActions = new();

    private readonly Dictionary<short, Action<GameObjectContext>> _scriptableGameObjectActions = new();

    private readonly Dictionary<short, (double interval, Action<GameObjectContext> callback)>
        _scriptableScheduledGameObjectActions = new();


    public ItemService(ILogger<ItemService> logger, IServiceProvider serviceProvider, ITypeService typeService) : base(
        logger
    )
    {
        _serviceProvider = serviceProvider;
        _typeService = typeService;
    }

    protected override async ValueTask<bool> StartAsync()
    {
        await ScanGameObjectActionsAsync();
        Engine.SchedulerService.OnTick += SchedulerService_OnTickAsync;
        SubscribeToEvent<GameObjectAddedEvent>(OnGameObjectAdded);
        SubscribeToEvent<GameObjectRemovedEvent>(OnGameObjectRemoved);

        return true;
    }

    private void OnGameObjectRemoved(GameObjectRemovedEvent obj)
    {
        if (obj.Layer == MapLayer.Objects)
        {
            _ = Task.Run(() => RemoveGameObjectAsync(obj));
        }
    }

    private void OnGameObjectAdded(GameObjectAddedEvent obj)
    {
        if (obj.Layer == MapLayer.Objects)
        {
            _ = Task.Run(
                async () =>
                {
                    await AddGameObjectActionAsync(obj);
                    await AddScriptableGameObjectAction(obj);
                }
            );
        }
    }

    private async ValueTask RemoveGameObjectAsync(GameObjectRemovedEvent @event)
    {
        await _gameObjectActionLock.WaitAsync();
        if (_gameObjectActions.TryGetValue(@event.Id, out var gameObjectAction))
        {
            GC.SuppressFinalize(gameObjectAction);
            _gameObjectActions.Remove(@event.Id);
        }

        if (_scheduledGameObjectActions.TryGetValue(@event.Id, out var scheduledObjectAction))
        {
            GC.SuppressFinalize(scheduledObjectAction);
            _scheduledGameObjectActions.Remove(@event.Id);
        }

        _gameObjectActionLock.Release();
    }

    private async ValueTask AddScriptableGameObjectAction(GameObjectAddedEvent @event)
    {
        var gameObjectEntity =
            await Engine.DatabaseService.QueryAsSingleAsync<GameObjectEntity>(
                entity => entity.Id == @event.ObjectId
            );

        var worldGameObject =
            await Engine.WorldService.GetEntityBySerialIdAsync<WorldGameObject>(@event.MapId, @event.Id);

        if (_scriptableGameObjectActions.TryGetValue(gameObjectEntity.GameObjectType, out var action))
        {
            var scriptableGameObjectAction = new BaseScriptableWorldObjectAction(
                _serviceProvider.GetRequiredService<ILogger<BaseScriptableWorldObjectAction>>(),
                Engine,
                action
            );
            await scriptableGameObjectAction.OnInitializedAsync(@event.MapId, worldGameObject!);
            _gameObjectActions.Add(worldGameObject!.ID, scriptableGameObjectAction);
        }

        if (_scriptableScheduledGameObjectActions.TryGetValue(gameObjectEntity.GameObjectType, out var scheduledAction))
        {
            var scriptableScheduledGameObjectAction = new BaseScriptableScheduledWorldObjectAction(
                _serviceProvider.GetRequiredService<ILogger<BaseScriptableScheduledWorldObjectAction>>(),
                Engine,
                scheduledAction.callback
            );
            scriptableScheduledGameObjectAction.SetScheduledInterval(scheduledAction.interval);
            await scriptableScheduledGameObjectAction.OnInitializedAsync(@event.MapId, worldGameObject!);
            _gameObjectActions.Add(worldGameObject!.ID, scriptableScheduledGameObjectAction);
        }
    }

    private async ValueTask AddGameObjectActionAsync(GameObjectAddedEvent @event)
    {
        var gameObjectEntity =
            await Engine.DatabaseService.QueryAsSingleAsync<GameObjectEntity>(
                entity => entity.Id == @event.ObjectId
            );

        if (_gameObjectActionTypes.TryGetValue(
                _typeService.GetGameObjectType(gameObjectEntity.GameObjectType)!.Name,
                out var type
            ))
        {
            var worldGameObject =
                await Engine.WorldService.GetEntityBySerialIdAsync<WorldGameObject>(@event.MapId, @event.Id);
            if (_serviceProvider.GetService(type) is IGameObjectAction gameObjectAction)
            {
                await _gameObjectActionLock.WaitAsync();
                try
                {
                    if (gameObjectAction is IScheduledGameObjectAction scheduledGameObjectAction)
                    {
                        _scheduledGameObjectActions.Add(worldGameObject!.ID, scheduledGameObjectAction);
                    }

                    await gameObjectAction.OnInitializedAsync(@event.MapId, worldGameObject!);
                    _gameObjectActions.Add(worldGameObject!.ID, gameObjectAction);
                }
                finally
                {
                    _gameObjectActionLock.Release();
                }
            }
        }
    }

    private ValueTask ScanGameObjectActionsAsync()
    {
        foreach (var type in AssemblyUtils.GetAttribute<GameObjectActionAttribute>())
        {
            try
            {
                var attr = type.GetCustomAttribute<GameObjectActionAttribute>();

                // I try to initialize the gemeObject action here, but it fails
                if (_serviceProvider.GetService(type) is IGameObjectAction gameObjectAction)
                {
                    _gameObjectActionTypes.Add(attr!.GameObjectType, type);
                    Logger.LogDebug("Added game object action {GameObjectType}", type.Name);
                    GC.SuppressFinalize(gameObjectAction);
                }
                else
                {
                    Logger.LogError(
                        "Failed to add game object action {GameObjectType}, maybe not implement interface IGameObjectAction?",
                        type
                    );
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Error during add game object action {GameObjectType}: {Error}", type, e);
            }
        }

        return ValueTask.CompletedTask;
    }

    private async Task SchedulerService_OnTickAsync(double deltaTime)
    {
        await _gameObjectActionLock.WaitAsync();
        foreach (var scheduledGameObjectAction in _scheduledGameObjectActions)
        {
            await scheduledGameObjectAction.Value.UpdateAsync(deltaTime);
        }

        _gameObjectActionLock.Release();
    }

    public Task ExecuteGameObjectActionAsync(
        WorldGameObject gameObject, string mapId, Guid? sessionId, Guid? playerId, bool isNpc, uint? npcId, Guid? npcObjectId
    )
    {
        if (_gameObjectActions.TryGetValue(gameObject.ID, out var action))
        {
            var senderId = Guid.NewGuid();
            if (playerId == null)
            {
                senderId = npcObjectId.Value;
            }
            else
            {
                senderId = playerId.Value;
            }

            _ = action.OnActivatedAsync(mapId, gameObject, senderId, isNpc);
            return Task.CompletedTask;
        }

        Logger.LogWarning(
            "Can't find Game object action {GameObjectName} [{GameObjectId}] ",
            gameObject.Type,
            gameObject.ObjectId
        );
        return Task.CompletedTask;
    }

    public void AddScriptableGameObject(GameObjectType gameObjectType, Action<GameObjectContext> callBack)
    {
        _scriptableGameObjectActions.Add(gameObjectType.Id, callBack);
    }

    public void AddScriptableScheduledGameObject(
        GameObjectType gameObjectType, int delay, Action<GameObjectContext> callBack
    )
    {
        _scriptableScheduledGameObjectActions.Add(gameObjectType.Id, (delay, callBack));
    }
}
