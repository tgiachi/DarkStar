using DarkStar.Api.Engine.Data.Items;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Map.Entities;
using Microsoft.Extensions.Logging;

namespace DarkStar.Api.Engine.Items.WorldObjects.Base;

public class BaseScriptableScheduledWorldObjectAction : BaseScheduledWorldObject
{
    public Action<GameObjectContext> GameObjectAction { get; set; }

    public BaseScriptableScheduledWorldObjectAction(
        ILogger<BaseScriptableScheduledWorldObjectAction> logger, IDarkSunEngine engine, Action<GameObjectContext> callback
    ) : base(logger, engine) => GameObjectAction = callback;

    public void SetScheduledInterval(double interval)
    {
        SetInterval(interval);
    }

    public override ValueTask OnActivatedAsync(string mapId, WorldGameObject gameObject, Guid senderId, bool isNpc)
    {
        GameObjectAction?.Invoke(
            new GameObjectContext()
            {
                GameObject = GameObject,
                MapId = mapId,
                SenderId = Guid.Empty,
                IsNpc = false
            }
        );
        return ValueTask.CompletedTask;
    }
}
