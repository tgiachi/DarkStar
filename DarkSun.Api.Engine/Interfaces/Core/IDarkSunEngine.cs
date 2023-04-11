using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Network.Server.Interfaces;
using Redbus.Interfaces;

namespace DarkSun.Api.Engine.Interfaces.Core;

public interface IDarkSunEngine
{
    IWorldService WorldService { get; }
    IBlueprintService BlueprintService { get; }
    ISchedulerService SchedulerService { get; }
    IScriptEngineService ScriptEngineService { get; }
    IDarkSunNetworkServer NetworkServer { get; }
    IPlayerSessionService PlayerSessionService { get; }
    IDatabaseService DatabaseService { get; }
    IEventBus EventBus { get; }
    ValueTask<bool> StartAsync();
    ValueTask<bool> StopAsync();
}
