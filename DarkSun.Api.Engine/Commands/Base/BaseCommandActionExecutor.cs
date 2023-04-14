using DarkSun.Api.Engine.Interfaces.Commands;
using DarkSun.Api.Engine.Interfaces.Core;
using Microsoft.Extensions.Logging;

namespace DarkSun.Api.Engine.Commands.Base;

public abstract class BaseCommandActionExecutor<TAction> : ICommandActionExecutor where TAction : ICommandAction
{
    protected ILogger Logger { get; }
    protected IDarkSunEngine Engine { get; }

    protected BaseCommandActionExecutor(ILogger<BaseCommandActionExecutor<TAction>> logger, IDarkSunEngine engine)
    {
        Logger = logger;
        Engine = engine;
    }

    public Task ProcessAsync(ICommandAction action)
    {
        return ProcessAsync((TAction)action);
    }
    public virtual Task ProcessAsync(TAction action)
    {
        return Task.CompletedTask;
    }
}
