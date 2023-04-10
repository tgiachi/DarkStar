using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Data.Config;
using DarkSun.Engine.Interfaces.Core;
using DarkSun.Engine.Interfaces.Services;
using DarkSun.Network.Server.Interfaces;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine
{
    public class DarkSunEngine : IDarkSunEngine
    {
        private readonly ILogger _logger;
        private readonly DirectoriesConfig _directoriesConfig;

        public IWorldService WorldService { get; }
        public IBlueprintService BlueprintService { get; }
        public ISchedulerService SchedulerService { get; }
        public IScriptEngineService ScriptEngineService { get; }
        public IDarkSunNetworkServer NetworkServer { get; }

        public DarkSunEngine(ILogger<DarkSunEngine> logger, 
            DirectoriesConfig directoriesConfig, 
            IWorldService worldService, 
            IBlueprintService blueprintService, 
            ISchedulerService schedulerService, 
            IScriptEngineService scriptEngineService, 
            IDarkSunNetworkServer networkServer)
        {
            _logger = logger;
            _directoriesConfig = directoriesConfig;
            WorldService = worldService;
            BlueprintService = blueprintService;
            SchedulerService = schedulerService;
            ScriptEngineService = scriptEngineService;
            NetworkServer = networkServer;
        }

       
    }
}
