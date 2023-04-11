using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Engine.Services.Base;
using Humanizer;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.Services;

[DarkSunEngineService("DiagnosticService", 1000)]
public class DiagnosticService : BaseService<DiagnosticService>, IDiagnosticService
{
    private static readonly CancellationTokenSource s_diagnosticCancellationTokenSource = new();
    private readonly CancellationToken _diagnosticCancellationToken = s_diagnosticCancellationTokenSource.Token;

    public DiagnosticService(ILogger<DiagnosticService> logger) : base(logger)
    {
    }

    protected override ValueTask<bool> StartAsync()
    {
        _ = Task.Factory.StartNew(StartDiagnosticTaskAsync, _diagnosticCancellationToken,
            TaskCreationOptions.LongRunning, TaskScheduler.Current);
        return base.StartAsync();
    }

    private async Task StartDiagnosticTaskAsync()
    {
        var currentProcess = Process.GetCurrentProcess();
        while (!s_diagnosticCancellationTokenSource.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(60), _diagnosticCancellationToken);
            Logger.LogInformation("Memory usage private: {Private} Paged: {Paged} Total Threads: {Threads}",
                currentProcess.PrivateMemorySize64.Bytes(), currentProcess.PagedMemorySize64.Bytes(),
                currentProcess.Threads.Count);
        }
    }
}
