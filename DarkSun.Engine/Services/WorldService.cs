using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Engine.Data.Config;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Engine.Services.Base;
using GoRogue.GameFramework;

using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.Services
{
    [DarkSunEngineService("WorldService", 10)]
    public class WorldService : BaseService<IWorldService>, IWorldService
    {
        private EngineConfig _engineConfig;

        public WorldService(ILogger<WorldService> logger, EngineConfig engineConfig) : base(logger)
        {
            _engineConfig = engineConfig;
        }

        public override  ValueTask<bool> StopAsync()
        {
            // await Task.Delay(3000);

            return ValueTask.FromResult(true);
        }
    }
}
