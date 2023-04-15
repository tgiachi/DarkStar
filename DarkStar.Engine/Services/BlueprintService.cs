using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Engine.Services.Base;

using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Services
{
    [DarkStarEngineService(nameof(BlueprintService), 2)]
    public class BlueprintService : BaseService<BlueprintService>, IBlueprintService
    {
        public BlueprintService(ILogger<BlueprintService> logger) : base(logger)
        {
        }
    }
}
