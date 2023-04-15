using DarkStar.Api.Engine.Types.Commands;

namespace DarkStar.Api.Engine.Interfaces.Commands;

public interface ICommandAction
{
    double Tick { get; set; }
    CommandActionType Type { get; }
}
