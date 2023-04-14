using DarkSun.Api.Engine.Types.Commands;

namespace DarkSun.Api.Engine.Interfaces.Commands;

public interface ICommandAction
{
    double Tick { get; set; }
    CommandActionType Type { get; }
}
