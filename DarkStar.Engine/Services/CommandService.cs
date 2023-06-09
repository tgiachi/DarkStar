using System.Diagnostics;
using System.Reflection;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Engine.Attributes.Commands;
using DarkStar.Api.Engine.Events.Commands;
using DarkStar.Api.Engine.Interfaces.Commands;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.Types.Commands;
using DarkStar.Api.Utils;
using DarkStar.Engine.Commands.Actions;
using DarkStar.Engine.Services.Base;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Services;

[DarkStarEngineService(nameof(CommandService), 13)]
public class CommandService : BaseService<ICommandService>, ICommandService
{
    private readonly Dictionary<CommandActionType, ICommandActionExecutor> _actionExecutors = new();
    private readonly List<ICommandAction> _playersActionsQueue = new();
    private readonly List<ICommandAction> _npcsActionsQueue = new();
    private readonly IServiceProvider _container;
    private readonly SemaphoreSlim _actionListLock = new(1);

    public CommandService(ILogger<CommandService> logger, IServiceProvider container) : base(logger) =>
        _container = container;

    protected override ValueTask<bool> StartAsync()
    {
        PrepareSchedulerExecutors();
        SubscribeToEvent<GameObjectCommandEvent>(OnGameObjectCommandEvent);
        Engine.SchedulerService.OnTick += SchedulerServiceOnOnTickAsync;
        return base.StartAsync();
    }

    private void OnGameObjectCommandEvent(GameObjectCommandEvent obj)
    {
        if (obj.IsNpc)
        {
            EnqueueNpcAction(
                new GameObjectAction
                {
                    IsNpc = obj.IsNpc,
                    MapId = obj.MapId,
                    NpcId = obj.NpcId,
                    NpcObjectId = obj.NpcObjectId,
                    Position = obj.Position,
                    SessionId = obj.SessionId,
                    Delay = obj.Delay,
                    PlayerId = obj.PlayerId
                }
            );
        }
    }

    private void PrepareSchedulerExecutors()
    {
        AssemblyUtils.GetAttribute<CommandActionAttribute>()
            .ForEach(
                s =>
                {
                    var attr = s.GetCustomAttribute<CommandActionAttribute>()!;
                    Logger.LogInformation("Adding {GameObjectType} from {ActionType}", s.Name, attr.Type);
                    var executor = _container.GetService(s);
                    _actionExecutors.Add(attr.Type, (ICommandActionExecutor)executor!);
                }
            );
    }

    public void EnqueuePlayerAction<ActionEntity>(ActionEntity entity) where ActionEntity : ICommandAction
    {
        _actionListLock.Wait();
        _playersActionsQueue.Add(entity);
        _actionListLock.Release();
    }

    public void EnqueueNpcAction<ActionEntity>(ActionEntity entity) where ActionEntity : ICommandAction
    {
        _actionListLock.Wait();
        _npcsActionsQueue.Add(entity);
        _actionListLock.Release();
    }

    private async Task SchedulerServiceOnOnTickAsync(double deltaTime)
    {
        await _actionListLock.WaitAsync();

        var actionsToRemove = new List<ICommandAction>();
        foreach (var action in _playersActionsQueue)
        {
            action.Delay -= deltaTime;

            if (action.Delay <= 0)
            {
                await ProcessPlayerActionAsync(action);
                actionsToRemove.Add(action);
            }
        }

        foreach (var action in actionsToRemove)
        {
            _playersActionsQueue.Remove(action);
        }

        actionsToRemove.Clear();

        foreach (var action in _npcsActionsQueue)
        {
            action.Delay -= deltaTime;
            if (action.Delay <= 0)
            {
                await ProcessPlayerActionAsync(action);
                actionsToRemove.Add(action);
            }
        }

        actionsToRemove.ForEach(k => _npcsActionsQueue.Remove(k));

        _actionListLock.Release();
    }

    private async Task ProcessPlayerActionAsync(ICommandAction action)
    {
        try
        {
            if (_actionExecutors.TryGetValue(action.Type, out var executor))
            {
                await executor.ProcessAsync(action);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError("Error during executing action: {GameObjectType}: {Ex}", action.Type, ex);
        }
    }
}
