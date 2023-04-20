using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Core;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace DarkStar.Engine.Runner;

public class DarkSunEngineHostedService : IHostedService
{
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceProvider _serviceProvider;

    public DarkSunEngineHostedService(ILogger<DarkSunEngineHostedService> logger,
        IHostApplicationLifetime lifetime,
        IServiceProvider serviceProvider)
    {
        _applicationLifetime = lifetime;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _serviceProvider.GetRequiredService<IDarkSunEngine>().StartAsync();

        Program.StartupStopwatch.Stop();
        Log.Logger.Information("Engine startup in {0} ms", Program.StartupStopwatch.ElapsedMilliseconds);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _serviceProvider.GetRequiredService<IDarkSunEngine>().StopAsync();
    }
}
