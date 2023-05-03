using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.ScriptModules;
using DarkStar.Engine.Attributes.ScriptEngine;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.ScriptModules;

[ScriptModule]
public class EventScriptModule : BaseScriptModule
{
    private readonly IEventDispatcherService _eventDispatcherService;

    public EventScriptModule(
        ILogger<EventScriptModule> logger, IDarkSunEngine engine, IEventDispatcherService eventDispatcherService
    ) : base(logger, engine) => _eventDispatcherService = eventDispatcherService;

    [ScriptFunction("on_engine_ready_event", "Adds an event to the engine ready event.")]
    public void AddEvent(Action callBack)
    {
        _eventDispatcherService.AddEngineReadyEvent(callBack);
    }
}
