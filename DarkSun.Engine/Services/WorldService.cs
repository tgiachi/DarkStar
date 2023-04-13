using System.Security.Cryptography.X509Certificates;
using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Api.World.Types.Map;
using DarkSun.Engine.Services.Base;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.Services
{
    [DarkSunEngineService("WorldService", 10)]
    public class WorldService : BaseService<IWorldService>, IWorldService
    {
        public WorldService(ILogger<WorldService> logger) : base(logger)
        {
        }

        public Task<Map> GenerateCityMap()
        {
            //var map = new Map(100, 100, Enum.GetNames<MapLayer>().Length, Distance.CHEBYSHEV);

            return Task.FromResult<Map>(null);
        }

        public override async ValueTask<bool> StopAsync()
        {
            // await Task.Delay(3000);

            return true;
        }
    }
}
