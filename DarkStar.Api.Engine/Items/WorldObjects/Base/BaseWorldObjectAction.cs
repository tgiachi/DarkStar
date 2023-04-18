using DarkStar.Api.Engine.Interfaces.Objects;
using DarkStar.Api.Engine.Map.Entities;
using Microsoft.Extensions.Logging;

namespace DarkStar.Api.Engine.Items.WorldObjects.Base
{
    public class BaseWorldObjectAction : IGameObjectAction
    {
        protected ILogger Logger { get; }
        protected string MapId { get; private set; } = null!;
        protected WorldGameObject GameObject { get; private set; } = null!;
        public BaseWorldObjectAction(ILogger<BaseWorldObjectAction> logger)
        {
            Logger = logger;
        }

        public ValueTask OnInitializedAsync(string mapId, WorldGameObject gameObject)
        {
            MapId = mapId;
            GameObject = gameObject;
            Logger.LogDebug("Initialized {GameObject}", gameObject.Tile);
            return ValueTask.CompletedTask;
        }

        public virtual ValueTask OnActivatedAsync(string mapId, WorldGameObject gameObject, Guid senderId, bool isNpc)
        {
            return ValueTask.CompletedTask;
        }

        public virtual void Dispose()
        {
        }
    }
}
