using DarkStar.Api.Engine.Data.Items;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Map.Entities;
using Microsoft.Extensions.Logging;

namespace DarkStar.Api.Engine.Items.WorldObjects.Base;

public class BaseScriptableWorldObjectAction : BaseWorldObjectAction
{
    public Action<GameObjectContext> GameObjectAction { get; set; }

    public BaseScriptableWorldObjectAction(
        ILogger<BaseScriptableWorldObjectAction> logger, IDarkSunEngine engine, Action<GameObjectContext> callback
    ) : base(
        logger,
        engine
    ) => GameObjectAction = callback;

    public override ValueTask OnActivatedAsync(string mapId, WorldGameObject gameObject, Guid senderId, bool isNpc)
    {
        GameObjectAction?.Invoke(
            new GameObjectContext
            {
                MapId = mapId,
                GameObject = gameObject,
                SenderId = senderId,
                IsNpc = isNpc
            }
        );
        return ValueTask.CompletedTask;
    }
}
