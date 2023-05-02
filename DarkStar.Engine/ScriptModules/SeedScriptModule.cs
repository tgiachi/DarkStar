using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.ScriptModules;
using DarkStar.Engine.Attributes.ScriptEngine;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.ScriptModules;

[ScriptModule]
public class SeedScriptModule : BaseScriptModule
{
    private readonly ISeedService _seedService;

    public SeedScriptModule(ILogger<SeedScriptModule> logger, IDarkSunEngine engine, ISeedService seedService) : base(
        logger,
        engine
    ) => _seedService = seedService;

    [ScriptFunction("add_text_content")]
    public void AddTextContent(string id, string content)
    {
        _seedService.AddTextContentSeed(id, content);
        Logger.LogDebug("Added text content seed {Id}", id.ToUpper());
    }
}
