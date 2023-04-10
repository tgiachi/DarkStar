using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Network.Server.Interfaces;

namespace DarkSun.Api.Engine.Interfaces.Core
{
    public interface IDarkSunEngine
    {
        IWorldService WorldService { get; }
        IBlueprintService BlueprintService { get; }
        ISchedulerService SchedulerService { get; }
        IScriptEngineService ScriptEngineService { get; }
        IDarkSunNetworkServer NetworkServer { get; }

        ValueTask<bool> StartAsync();
        ValueTask<bool> StopAsync();
    }
}
