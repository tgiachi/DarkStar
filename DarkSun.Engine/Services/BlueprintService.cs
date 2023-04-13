using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Engine.Services.Base;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.Services
{
    [DarkSunEngineService(nameof(BlueprintService), 2)]
    public class BlueprintService : BaseService<BlueprintService>, IBlueprintService
    {
        public BlueprintService(ILogger<BlueprintService> logger) : base(logger)
        {
        }
    }
}
