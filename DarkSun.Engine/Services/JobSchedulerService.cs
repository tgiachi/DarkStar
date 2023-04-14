using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Engine.Services.Base;
using FluentScheduler;
using Humanizer;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.Services
{

    [DarkSunEngineService(nameof(JobSchedulerService), 3)]
    public class JobSchedulerService : BaseService<JobSchedulerService>, IJobSchedulerService
    {
        public JobSchedulerService(ILogger<JobSchedulerService> logger) : base(logger)
        {
        }

        protected override ValueTask<bool> StartAsync()
        {
            JobManager.Start();
            return base.StartAsync();
        }

        public void AddJob(string name, Action action, int seconds, bool runOnce)
        {
            Logger.LogDebug("Adding scheduled job: {Name} every {Seconds} seconds, runOne: {RunOnce}", name, seconds.Seconds(), runOnce);
            JobManager.AddJob(action, schedule => schedule.WithName(name).ToRunEvery(seconds).Seconds());
        }

        public void RemoveJob(string name)
        {
            JobManager.RemoveJob(name);
        }

        public override ValueTask<bool> StopAsync()
        {
            JobManager.StopAndBlock();
            return base.StopAsync();
        }
    }
}
