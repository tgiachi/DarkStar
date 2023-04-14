using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Data.Config;
using DarkStar.Api.Engine.Events.Engine;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Engine.Services.Base;

using Humanizer;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Services
{
    [DarkStarEngineService("DiagnosticService", 1000)]
    public class DiagnosticService : BaseService<DiagnosticService>, IDiagnosticService
    {
        private readonly string _pidFileName;

        public DiagnosticService(ILogger<DiagnosticService> logger, DirectoriesConfig directoriesConfig) : base(logger)
        {
            _pidFileName = Path.Join(directoriesConfig[DirectoryNameType.Root], "darksun.pid");
        }

        protected override ValueTask<bool> StartAsync()
        {

            if (File.Exists(_pidFileName))
            {
                Logger.LogWarning("!!! PID Exists, server did't shutdown correctly!");
            }

            Engine.EventBus.Subscribe<EngineReadyEvent>(OnEngineReady);
            Engine.JobSchedulerService.AddJob("DiagnosticService", StartDiagnosticJob, 60, false);
            return base.StartAsync();
        }

        public override ValueTask<bool> StopAsync()
        {
            File.Delete(_pidFileName);
            return base.StopAsync();
        }

        private void OnEngineReady(EngineReadyEvent obj)
        {
            File.WriteAllText(_pidFileName, Process.GetCurrentProcess().Id.ToString());
        }

        private void StartDiagnosticJob()
        {
            var currentProcess = Process.GetCurrentProcess();

            Logger.LogInformation("Memory usage private: {Private} Paged: {Paged} Total Threads: {Threads} PID: {Pid}",
                currentProcess.PrivateMemorySize64.Bytes(), currentProcess.PagedMemorySize64.Bytes(),
                currentProcess.Threads.Count, currentProcess.Id);

        }
    }
}
