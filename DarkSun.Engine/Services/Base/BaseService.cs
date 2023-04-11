using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Interfaces.Core;
using DarkSun.Api.Engine.Interfaces.Services.Base;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.Services.Base;

public class BaseService<TService> : IDarkSunEngineService where TService : IDarkSunEngineService
{
    protected ILogger Logger { get; }
    protected IDarkSunEngine Engine { get; private set; } = null!;

    protected BaseService(ILogger<TService> logger)
    {
        Logger = logger;
    }

    public virtual ValueTask DisposeAsync()
    {
        Logger.LogDebug("Disposing service {Service}", GetType().Name);
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> StartAsync(IDarkSunEngine engine)
    {
        Logger.LogDebug("Starting service {Service}", GetType().Name);
        Engine = engine;
        return StartAsync();
    }

    protected virtual ValueTask<bool> StartAsync()
    {
        return ValueTask.FromResult(true);
    }

    public virtual ValueTask<bool> StopAsync()
    {
        Logger.LogDebug("Stopping service {Service}", GetType().Name);
        return new ValueTask<bool>(true);
    }
}
