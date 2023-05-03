using DarkStar.Api.Engine.Attributes.Commands;
using DarkStar.Api.Engine.Commands.Base;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.Engine.Types.Commands;
using DarkStar.Engine.Commands.Actions;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Commands.Executors;

[CommandAction(CommandActionType.GameObjectAction)]
public class GameObjectActionExecutor : BaseCommandActionExecutor<GameObjectAction>
{
    public GameObjectActionExecutor(
        ILogger<BaseCommandActionExecutor<GameObjectAction>> logger, IDarkSunEngine engine
    ) : base(logger, engine)
    {
    }

    public override async Task ProcessAsync(GameObjectAction action)
    {
        var worldObject = await Engine.WorldService.GetEntityByPositionAsync<WorldGameObject>(action.MapId, action.Position);
        if (worldObject != null)
        {
            await Engine.ItemService.ExecuteGameObjectActionAsync(
                worldObject,
                action.MapId,
                action.SessionId,
                action.PlayerId,
                action.IsNpc,
                action.NpcId,
                action.NpcObjectId
            );
        }
    }
}
