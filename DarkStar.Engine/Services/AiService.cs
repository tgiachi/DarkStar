using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Engine.Attributes.Ai;
using DarkStar.Api.Engine.Events.Map;
using DarkStar.Api.Engine.Interfaces.Ai;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.Utils;
using DarkStar.Api.World.Types.Map;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Database.Entities.Npc;
using DarkStar.Engine.Services.Base;

using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Services
{
    [DarkStarEngineService(nameof(AiService), 5)]
    public class AiService : BaseService<AiService>, IAiService
    {
        private readonly IServiceProvider _serviceProvider;
        private SemaphoreSlim _aiExecutorsLock = new(1);
        private Dictionary<uint, IAiBehaviourExecutor> _aiExecutors = new();

        private readonly List<(NpcType, NpcSubType, Type)> _aiBehaviourTypes =
            new();

        public AiService(ILogger<AiService> logger, IServiceProvider serviceProvider) : base(logger)
        {
            _serviceProvider = serviceProvider;
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

        private async ValueTask AddNpcAiAsync(GameObjectAddedEvent @event)
        {
            try
            {
                var npcGameObject = await Engine.WorldService.GetEntityByIdAsync<NpcGameObject>(@event.MapId, @event.ObjectId);
                var npcEntity = await Engine.DatabaseService.QueryAsSingleAsync<NpcEntity>(entity => entity.Id == @event.ObjectId);
                var (npcType, _, type) = _aiBehaviourTypes.FirstOrDefault(x => x.Item1 == npcEntity.Type && x.Item2 == npcEntity.SubType);
                if (npcType == default)
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
                Logger.LogError(e, "Failed to add ai to npc {Type}: {Error}", @event.ObjectId, e);
            }

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
                    Logger.LogError("Failed to create instance of {Type}, have you missing to implement IAiBehaviourExecutor interface?", type.Name);
                    continue;
                }

                Logger.LogInformation("Found behaviour {Type} for {NpcType} {NpcSubType}", type.Name, attr!.Type, attr.SubType);

                _aiBehaviourTypes.Add((attr.Type, attr.SubType, type));
                GC.SuppressFinalize(behaviour);

            }
            return ValueTask.CompletedTask;

        }
    }
}
