using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Engine.Services.Base;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.Services
{
    [DarkSunEngineService(nameof(CommandService), 13)]
    public class CommandService : BaseService<ICommandService>, ICommandService
    {
        public CommandService(ILogger<ICommandService> logger) : base(logger)
        {
        }

        protected override ValueTask<bool> StartAsync()
        {
            Engine.SchedulerService.OnTick += SchedulerServiceOnOnTickAsync;
            return base.StartAsync();
        }

        private Task SchedulerServiceOnOnTickAsync(double deltaTime)
        {
            return Task.CompletedTask;
        }
    }
}
