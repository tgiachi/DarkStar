using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Network.Server.Interfaces;
using Redbus.Interfaces;

namespace DarkSun.Api.Engine.Interfaces.Core;

public interface IDarkSunEngine
{
    string ServerName { get;set;  }
    string ServerMotd { get; set; }
    IWorldService WorldService { get; }
    IBlueprintService BlueprintService { get; }
    ISchedulerService SchedulerService { get; }
    IScriptEngineService ScriptEngineService { get; }
    IDarkSunNetworkServer NetworkServer { get; }
    IPlayerService PlayerService { get; }
    IDatabaseService DatabaseService { get; }
    ICommandService CommandService { get; }
    INamesService NamesService { get; }
    ISeedService SeedService { get; }
    IEventBus EventBus { get; }
    ValueTask<bool> StartAsync();
    ValueTask<bool> StopAsync();


}
