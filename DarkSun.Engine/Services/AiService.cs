using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Engine.Services.Base;

using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Services
{
    [DarkStarEngineService(nameof(AiService), 5)]
    public class AiService : BaseService<AiService>, IAiService
    {
        public AiService(ILogger<AiService> logger) : base(logger)
        {
        }

        protected override ValueTask<bool> StartAsync()
        {
            Engine.SchedulerService.OnTick += SchedulerOnOnTickAsync;
            return base.StartAsync();
        }

        private Task SchedulerOnOnTickAsync(double deltaTime)
        {
            return Task.CompletedTask;
        }
    }
}
