using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Engine.Services.Base;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.Services
{
    [DarkSunEngineService("WorldService", 5)]
    public class WorldService : BaseService<IWorldService>, IWorldService
    {
        public WorldService(ILogger<WorldService> logger) : base(logger)
        {
        }
    }
}
