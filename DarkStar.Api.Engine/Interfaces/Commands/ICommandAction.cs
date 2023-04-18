using DarkStar.Api.Engine.Types.Commands;

namespace DarkStar.Api.Engine.Interfaces.Commands;

public interface ICommandAction
{
    double Delay { get; set; }
    CommandActionType Type { get; }
}
