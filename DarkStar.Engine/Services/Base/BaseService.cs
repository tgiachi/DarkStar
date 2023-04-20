using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Interfaces.Services.Base;
using Microsoft.Extensions.Logging;
using Redbus;
using Redbus.Events;

namespace DarkStar.Engine.Services.Base;

public class BaseService<TService> : IDarkSunEngineService where TService : IDarkSunEngineService
{
    protected ILogger Logger { get; }
    protected IDarkSunEngine Engine { get; private set; } = null!;

    private readonly List<SubscriptionToken> _eventBusSubscriptions = new();

    public BaseService(ILogger<TService> logger)
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

    protected void SubscribeToEvent<TEvent>(Action<TEvent> action) where TEvent : EventBase
    {
        _eventBusSubscriptions.Add(Engine.EventBus.Subscribe(action));
    }

    protected virtual ValueTask<bool> StartAsync()
    {
        return ValueTask.FromResult(true);
    }

    public virtual ValueTask<bool> StopAsync()
    {
        Logger.LogDebug("Stopping service {Service}", GetType().Name);
        _eventBusSubscriptions.ForEach(Engine.EventBus.Unsubscribe);
        return new ValueTask<bool>(true);
    }
}
