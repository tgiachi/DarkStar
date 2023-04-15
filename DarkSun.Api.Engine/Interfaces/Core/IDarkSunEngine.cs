using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Network.Server.Interfaces;
using Redbus.Interfaces;

namespace DarkStar.Api.Engine.Interfaces.Core;

public interface IDarkSunEngine
{
    string ServerName { get; set; }
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
    IJobSchedulerService JobSchedulerService { get; }
    IItemService ItemService { get; }
    IEventBus EventBus { get; }
    ValueTask<bool> StartAsync();
    ValueTask<bool> StopAsync();


}
