using DarkStar.Api.Engine.Attributes.Commands;
using DarkStar.Api.Engine.Commands.Base;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Types.Commands;
using DarkStar.Engine.Commands.Actions;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Commands.Executors;

[CommandAction(CommandActionType.GameObjectAction)]
public class GameObjectActionExecutor : BaseCommandActionExecutor<GameObjectAction>
{
    public GameObjectActionExecutor(ILogger<BaseCommandActionExecutor<GameObjectAction>> logger, IDarkSunEngine engine) : base(logger, engine)
    {

    }

    public override Task ProcessAsync(GameObjectAction action)
    {

        return Task.CompletedTask;
    }
}
