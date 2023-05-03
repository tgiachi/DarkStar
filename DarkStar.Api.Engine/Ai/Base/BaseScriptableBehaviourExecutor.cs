using DarkStar.Api.Engine.Data.Ai;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Database.Entities.Npc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DarkStar.Api.Engine.Ai.Base;

public class BaseScriptableBehaviourExecutor : BaseAiBehaviourExecutor
{
    private readonly IServiceProvider _serviceProvider;

    public AiContext Ai { get; set; }
    public Action<AiContext> ExecutorFunc { get; set; }

    public BaseScriptableBehaviourExecutor(
        ILogger<BaseScriptableBehaviourExecutor> logger, IDarkSunEngine engine, IServiceProvider serviceProvider
    ) : base(logger, engine) => _serviceProvider = serviceProvider;

    public override async ValueTask InitializeAsync(string mapId, NpcEntity npc, NpcGameObject npcGameObject)
    {
        await base.InitializeAsync(mapId, npc, npcGameObject);
        InitAiContext();
    }

    private void InitAiContext()
    {
        Ai = new AiContext
        {
            NpcGameObject = NpcGameObject,
            NpcEntity = NpcEntity,
            MapId = MapId,
            Logger = _serviceProvider.GetRequiredService<ILogger<AiContext>>()
        };
    }

    protected override ValueTask DoAiAsync()
    {
        ExecutorFunc.Invoke(Ai);
        return ValueTask.CompletedTask;
    }
}
