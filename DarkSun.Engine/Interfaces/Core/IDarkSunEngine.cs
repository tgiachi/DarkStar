using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Engine.Interfaces.Services;
using DarkSun.Network.Server.Interfaces;

namespace DarkSun.Engine.Interfaces.Core
{
    public interface IDarkSunEngine
    {
        IWorldService WorldService { get; }
        IBlueprintService BlueprintService { get; }
        ISchedulerService SchedulerService { get; }
        IScriptEngineService ScriptEngineService { get; }
        IDarkSunNetworkServer NetworkServer { get; }
    }
}
