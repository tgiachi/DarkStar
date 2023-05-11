using DarkStar.Api.Engine.Data.Ai;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.ScriptModules;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Engine.Attributes.ScriptEngine;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.ScriptModules;

[ScriptModule]
public class AiScriptModule : BaseScriptModule
{
    private readonly IAiService _aiService;
    private readonly ITypeService _typeService;

    public AiScriptModule(
        ILogger<AiScriptModule> logger, IDarkSunEngine engine, IAiService aiService, ITypeService typeService
    ) :
        base(logger, engine)
    {
        _aiService = aiService;
        _typeService = typeService;
    }

    [ScriptFunction("add_ai_brain_by_type")]
    public void AddAiScriptByType(string npcType, string npcSubType, Action<AiContext> context)
    {
        Logger.LogInformation("Adding AI script for {NpcType} {NpcSubType}", npcType, npcSubType);
        _aiService.AddAiScriptByType(
            _typeService.GetNpcType(npcType)!.Value,
            _typeService.GetNpcSubType(npcSubType),
            context
        );
    }


    [ScriptFunction("add_ai_brain_by_name")]
    public void AddAiScriptByName(string name, Action<AiContext> context) => _aiService.AddAiScriptByName(name, context);
}
