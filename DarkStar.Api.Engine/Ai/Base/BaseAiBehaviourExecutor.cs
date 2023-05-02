using DarkStar.Api.Engine.Interfaces.Ai;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.Engine.Map.Entities.Base;
using DarkStar.Api.Engine.Utils;
using DarkStar.Api.Utils;
using DarkStar.Api.World.Types.Map;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Database.Entities.Npc;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Messages.World;
using GoRogue.GameFramework;
using Microsoft.Extensions.Logging;
using SadRogue.Primitives;

namespace DarkStar.Api.Engine.Ai.Base;

public class BaseAiBehaviourExecutor : IAiBehaviourExecutor
{

    /// <summary>
    /// Default interval is 1 second
    /// </summary>
    public double Interval { get; set; } = 1000;
    private double _currentInterval = 1000;
    protected NpcGameObject NpcGameObject { get; private set; } = null!;
    protected NpcEntity NpcEntity { get; private set; } = null!;
    protected string MapId { get; private set; } = null!;

    protected ILogger Logger { get; }
    protected IDarkSunEngine Engine { get; }

    public BaseAiBehaviourExecutor(ILogger<BaseAiBehaviourExecutor> logger, IDarkSunEngine engine)
    {
        Logger = logger;
        Engine = engine;
    }

    public ValueTask ProcessAsync(double delta)
    {
        _currentInterval -= delta;
        if (!(_currentInterval <= 0))
        {
            return ValueTask.CompletedTask;
        }

        _currentInterval = Interval;
        return DoAiAsync();
    }

    public virtual ValueTask InitializeAsync(string mapId, NpcEntity npc, NpcGameObject npcGameObject)
    {
        Logger.LogDebug("Initializing {Name} AI Behaviour for {GameObjectType} {SubType} {Alignment} ID: {Id}", GetType().Name, npc.Type, npc.SubType, npc.Alignment, npcGameObject.ID);
        MapId = mapId;
        NpcGameObject = npcGameObject;
        NpcEntity = npc;
        return ValueTask.CompletedTask;
    }

    protected void SetInterval(double interval)
    {
        Interval = interval;
        _currentInterval = interval;
    }

    protected virtual ValueTask DoAiAsync() => ValueTask.CompletedTask;

    public virtual void Dispose()
    {

    }

    protected bool MoveToDirection(MoveDirectionType direction)
    {
        var newPosition = NpcGameObject.Position.ToPointPosition().AddMovement(direction);
        var canMove = Engine.WorldService.IsLocationWalkable(MapId, newPosition);
        if (canMove)
        {
            NpcGameObject.Position = newPosition.ToPoint();
            return true;
        }
        return false;
    }

    protected bool MoveRandomDirection() => MoveToDirection(MoveDirectionType.East.RandomEnumValue());

    protected Task<List<TEntity>> GetEntitiesInRangeAsync<TEntity>(MapLayer layer, int range = 5) where TEntity : BaseGameObject =>
        Engine.WorldService.GetEntitiesInRangeAsync<TEntity>(
            MapId,
            layer,
            NpcGameObject.Position.ToPointPosition(),
            range
        );

    protected List<PointPosition> GetPathToPosition(PointPosition position) =>
        Engine.WorldService.CalculateAStarPath(MapId, NpcGameObject.Position.ToPointPosition(), position);

    protected bool MoveToPosition(PointPosition position)
    {
        if (Engine.WorldService.IsLocationWalkable(MapId, position))
        {
            NpcGameObject.Position = new Point(position.X, position.Y);

            return true;
        }
        return false;
    }

    protected Task<bool> SendWorldMessageAsync(string message, WorldMessageType type) =>
        Engine.PlayerService.BroadcastChatMessageAsync(
            MapId,
            NpcGameObject.PointPosition(),
            NpcEntity.Name,
            NpcGameObject.ID,
            message,
            type
        );
}
