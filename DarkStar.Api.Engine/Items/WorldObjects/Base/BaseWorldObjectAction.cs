using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Interfaces.Objects;
using DarkStar.Api.Engine.Map.Entities;
using Microsoft.Extensions.Logging;

namespace DarkStar.Api.Engine.Items.WorldObjects.Base;

public class BaseWorldObjectAction : IGameObjectAction
{
    protected ILogger Logger { get; }
    protected IDarkSunEngine Engine { get; }
    protected string MapId { get; private set; } = null!;
    protected WorldGameObject GameObject { get; private set; } = null!;

    public BaseWorldObjectAction(ILogger<BaseWorldObjectAction> logger, IDarkSunEngine engine)
    {
        Logger = logger;
        Engine = engine;
    }

    public ValueTask OnInitializedAsync(string mapId, WorldGameObject gameObject)
    {
        MapId = mapId;
        GameObject = gameObject;
        return ValueTask.CompletedTask;
    }

    public virtual ValueTask OnActivatedAsync(string mapId, WorldGameObject gameObject, Guid senderId, bool isNpc) =>
        ValueTask.CompletedTask;

    protected ValueTask RemoveMySelfAsync() => Engine.WorldService.RemoveEntityAsync(MapId, GameObject.ID);

    public virtual void Dispose()
    {
    }
}
