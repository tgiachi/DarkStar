using System.Reflection;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Engine.Ai.Base;
using DarkStar.Api.Engine.Attributes.Ai;
using DarkStar.Api.Engine.Data.Ai;
using DarkStar.Api.Engine.Events.Map;
using DarkStar.Api.Engine.Interfaces.Ai;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.Utils;
using DarkStar.Api.World.Types.Map;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Database.Entities.Npc;
using DarkStar.Engine.Services.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Services;

[DarkStarEngineService(nameof(AiService), 9)]
public class AiService : BaseService<AiService>, IAiService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITypeService _typeService;
    private readonly SemaphoreSlim _aiExecutorsLock = new(1);
    private readonly Dictionary<uint, IAiBehaviourExecutor> _aiExecutors = new();
    private readonly List<(short npcType, short npcSubType, Action<AiContext>)> _aiScriptableTypesExecutors = new();
    private readonly Dictionary<string, Action<AiContext>> _aiScriptableNamesExecutors = new();

    private readonly List<(short npcType, short npcSubType, Type)> _aiBehaviourTypes =
        new();

    public AiService(ILogger<AiService> logger, IServiceProvider serviceProvider, ITypeService typeService) : base(logger)
    {
        _serviceProvider = serviceProvider;
        _typeService = typeService;
    }

    protected override async ValueTask<bool> StartAsync()
    {
        await ScanForAiBehaviourAsync();
        SubscribeToEvent<GameObjectAddedEvent>(OnGameObjectAddedEvent);
        Engine.SchedulerService.OnTick += SchedulerOnOnTickAsync;

        return true;
    }

    private void OnGameObjectAddedEvent(GameObjectAddedEvent obj)
    {
        if (obj.Layer == MapLayer.Creatures)
        {
            _ = Task.Run(() => AddNpcAiAsync(obj));
        }
    }

    private async ValueTask AddClassNpcAiAsync(GameObjectAddedEvent @event)
    {
        try
        {
            var npcGameObject = await Engine.WorldService.GetEntityByIdAsync<NpcGameObject>(@event.MapId, @event.ObjectId);
            var npcEntity =
                await Engine.DatabaseService.QueryAsSingleAsync<NpcEntity>(entity => entity.Id == @event.ObjectId);
            var (npcType, _, type) =
                _aiBehaviourTypes.FirstOrDefault(x => x.Item1 == npcEntity.Type && x.Item2 == npcEntity.SubType);
            if (type == null!)
            {
                Logger.LogWarning("No ai behaviour found for {NpcType} {NpcSubType}", npcEntity.Type, npcEntity.SubType);
                return;
            }

            await _aiExecutorsLock.WaitAsync();
            var executor = _serviceProvider.GetService(type) as IAiBehaviourExecutor;
            await executor!.InitializeAsync(@event.MapId, npcEntity, npcGameObject!);
            _aiExecutors.Add(npcGameObject!.ID, executor!);
            _aiExecutorsLock.Release();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to add ai to npc {GameObjectType}: {Error}", @event.ObjectId, e);
        }
    }

    private async ValueTask AddScriptNpc(GameObjectAddedEvent @event)
    {
        try
        {
            var npcGameObject = await Engine.WorldService.GetEntityByIdAsync<NpcGameObject>(@event.MapId, @event.ObjectId);
            var npcEntity =
                await Engine.DatabaseService.QueryAsSingleAsync<NpcEntity>(entity => entity.Id == @event.ObjectId);
            var script = _aiScriptableTypesExecutors.FirstOrDefault(
                s => s.npcType == npcEntity.Type && s.npcSubType == npcEntity.SubType
            );

            if (script.Item3 != null)
            {
                Logger.LogDebug("Adding scriptable ai to npc {GameObjectType}", @event.ObjectId);

                await _aiExecutorsLock.WaitAsync();
                var executor = new BaseScriptableBehaviourExecutor(
                    _serviceProvider.GetRequiredService<ILogger<BaseScriptableBehaviourExecutor>>(),
                    Engine,
                    _serviceProvider
                );
                executor!.ExecutorFunc = script.Item3;

                await executor!.InitializeAsync(@event.MapId, npcEntity, npcGameObject!);


                _aiExecutors.Add(npcGameObject!.ID, executor!);
                _aiExecutorsLock.Release();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to add scriptable ai to npc {GameObjectType}: {Error}", @event.ObjectId, ex);
        }
    }


    private async ValueTask AddNpcAiAsync(GameObjectAddedEvent @event)
    {
        await AddClassNpcAiAsync(@event);
        await AddScriptNpc(@event);
    }

    private async Task SchedulerOnOnTickAsync(double deltaTime)
    {
        await _aiExecutorsLock.WaitAsync();

        foreach (var executor in _aiExecutors.Values)
        {
            await executor.ProcessAsync(deltaTime);
        }

        _aiExecutorsLock.Release();
    }

    private ValueTask ScanForAiBehaviourAsync()
    {
        foreach (var type in AssemblyUtils.GetAttribute<AiBehaviourAttribute>())
        {
            var attr = type.GetCustomAttribute<AiBehaviourAttribute>();

            if (_serviceProvider.GetService(type) is not IAiBehaviourExecutor behaviour)
            {
                Logger.LogError(
                    "Failed to create instance of {GameObjectType}, have you missing to implement IAiBehaviourExecutor interface?",
                    type.Name
                );
                continue;
            }

            Logger.LogInformation(
                "Found behaviour {GameObjectType} for {NpcType} {NpcSubType}",
                type.Name,
                attr!.NpcType,
                attr.NpcSubType
            );
            var npcType = _typeService.GetNpcType(attr.NpcType);
            var npcSubType = _typeService.GetNpcSubType(attr.NpcSubType);


            _aiBehaviourTypes.Add((npcType.Value.Id, npcSubType.Id, type));

            GC.SuppressFinalize(behaviour);
        }

        return ValueTask.CompletedTask;
    }

    public void AddAiScriptByType(NpcType npcType, NpcSubType npcSubType, Action<AiContext> context)
    {
        _aiScriptableTypesExecutors.Add((npcType.Id, npcSubType.Id, context));
    }

    public void AddAiScriptByName(string name, Action<AiContext> context)
    {
        _aiScriptableNamesExecutors.Add(name, context);
    }
}
