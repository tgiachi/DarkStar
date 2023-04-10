using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Data.Config;
using DarkSun.Api.Engine.Data.Config;
using DarkSun.Api.Engine.Interfaces.Core;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Network.Server.Interfaces;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine
{
    public class DarkSunEngine : IDarkSunEngine
    {
        private readonly ILogger _logger;
        private readonly DirectoriesConfig _directoriesConfig;
        private readonly EngineConfig _engineConfig;

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
            IDarkSunNetworkServer networkServer,
            EngineConfig engineConfig)
        {
            _logger = logger;
            _directoriesConfig = directoriesConfig;
            WorldService = worldService;
            BlueprintService = blueprintService;
            SchedulerService = schedulerService;
            ScriptEngineService = scriptEngineService;
            NetworkServer = networkServer;
            _engineConfig = engineConfig;
        }

        public ValueTask<bool> StartAsync()
        {
            return new ValueTask<bool>(true);
        }

        public ValueTask<bool> StopAsync()
        {
            return new ValueTask<bool>(true);
        }
    }
}
