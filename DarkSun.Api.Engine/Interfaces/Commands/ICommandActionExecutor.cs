namespace DarkSun.Api.Engine.Interfaces.Commands;

public interface ICommandActionExecutor
{
    Task ProcessAsync(ICommandAction action);
}
