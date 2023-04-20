﻿using DarkStar.Api.Engine.Interfaces.Ai;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.Engine.Utils;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Database.Entities.Npc;
using DarkStar.Network.Protocol.Messages.Common;
using Microsoft.Extensions.Logging;

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

    public ValueTask InitializeAsync(string mapId, NpcEntity npc, NpcGameObject npcGameObject)
    {
        Logger.LogDebug("Initializing {Name} AI Behaviour for {Type} {SubType} {Alignment} ID: {Id}", GetType().Name, npc.Type, npc.SubType, npc.Alignment, npcGameObject.ID);
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
}
