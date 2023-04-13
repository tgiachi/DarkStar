using System;
using System.Collections.Generic;
using System.Diagnostics;
using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Engine.Services.Base;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.Services
{
    [DarkSunEngineService("SchedulerService", 3)]
    public class SchedulerService : BaseService<SchedulerService>, ISchedulerService
    {
        private const short Tick = 30;

        public event ISchedulerService.OnTickDelegate? OnTick;

        private static readonly CancellationTokenSource s_schedulerCancellationTokenSource = new();
        private readonly CancellationToken _schedulerCancellationToken = s_schedulerCancellationTokenSource.Token;

        public SchedulerService(ILogger<SchedulerService> logger) : base(logger)
        {
        }

        protected override ValueTask<bool> StartAsync()
        {
            Logger.LogInformation("Starting Scheduler");
            _ = Task.Factory.StartNew(ExecuteSchedulerTaskAsync, _schedulerCancellationToken,
                TaskCreationOptions.LongRunning, TaskScheduler.Current);
            return base.StartAsync();
        }

        private async Task ExecuteSchedulerTaskAsync()
        {
            while (!_schedulerCancellationToken.IsCancellationRequested)
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                await Task.Delay(Tick, _schedulerCancellationToken);
                if (OnTick != null)
                {
                    await OnTick?.Invoke(stopWatch.Elapsed.TotalMilliseconds)!;
                }

                stopWatch.Stop();
            }
        }

        public override ValueTask<bool> StopAsync()
        {
            s_schedulerCancellationTokenSource.Cancel();
            return base.StopAsync();
        }
    }
}
