using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Data.Config;
using DarkSun.Api.Engine.Data.Config;
using DarkSun.Api.Engine.Interfaces.Core;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Api.Engine.Interfaces.Services.Base;
using DarkSun.Api.Utils;
using DarkSun.Network.Server.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine
{
    public class DarkSunEngine : IDarkSunEngine
    {
        private readonly ILogger _logger;
        private readonly DirectoriesConfig _directoriesConfig;
        private readonly IServiceProvider _container;
        private readonly EngineConfig _engineConfig;
        private readonly SortedDictionary<int, IDarkSunEngineService> _servicesLoadOrder = new();

        public IWorldService WorldService { get; }
        public IBlueprintService BlueprintService { get; }
        public ISchedulerService SchedulerService { get; }
        public IScriptEngineService ScriptEngineService { get; }
        public IDarkSunNetworkServer NetworkServer { get; }

       
        public DarkSunEngine(ILogger<DarkSunEngine> logger,
            DirectoriesConfig directoriesConfig,
            IBlueprintService blueprintService,
            ISchedulerService schedulerService,
            IScriptEngineService scriptEngineService,
            IDarkSunNetworkServer networkServer,
            IWorldService worldService,
            EngineConfig engineConfig,
            IServiceProvider container)
        {
            _logger = logger;
            _directoriesConfig = directoriesConfig;
            WorldService = worldService;
            BlueprintService = blueprintService;
            SchedulerService = schedulerService;
            ScriptEngineService = scriptEngineService;
            NetworkServer = networkServer;
            _engineConfig = engineConfig;
            _container = container;
        }

        public async ValueTask<bool> StartAsync()
        {
            await BuildServicesOrderAsync();

            foreach (var service in _servicesLoadOrder)
            {
                await service.Value.StartAsync();
            }

            await NetworkServer.StartAsync();

            return true;
        }

        public async ValueTask<bool> StopAsync()
        {
            await NetworkServer.StopAsync();
            foreach (var service in _servicesLoadOrder.Reverse())
            {
                await service.Value.StopAsync();
            }

            return true;
        }

        private ValueTask BuildServicesOrderAsync()
        {
            _logger.LogDebug("Building services load order");
            var services = AssemblyUtils.GetAttribute<DarkSunEngineServiceAttribute>();
            foreach (var serviceType in services)
            {
                var attr = serviceType.GetCustomAttribute<DarkSunEngineServiceAttribute>()!;
                var interf = AssemblyUtils.GetInterfacesOfType(serviceType)!.First(k => k.Name.EndsWith(serviceType.Name));
                _servicesLoadOrder.Add(attr.LoadOrder, (IDarkSunEngineService)_container.GetService(interf)!);
            }


            return ValueTask.CompletedTask;
        }
    }
}
