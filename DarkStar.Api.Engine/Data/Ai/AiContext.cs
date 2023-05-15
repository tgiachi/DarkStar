using DarkStar.Api.Engine.Events.Commands;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.Engine.Map.Entities.Base;
using DarkStar.Api.Engine.Utils;
using DarkStar.Api.Utils;
using DarkStar.Api.World.Types.Map;
using DarkStar.Database.Entities.Npc;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Messages.World;
using Microsoft.Extensions.Logging;
using Redbus.Interfaces;

namespace DarkStar.Api.Engine.Data.Ai;

public class AiContext
{
    public NpcGameObject NpcGameObject { get; set; } = null!;
    public NpcEntity NpcEntity { get; set; } = null!;
    public string MapId { get; set; } = null!;
    public ILogger<AiContext> Logger { get; set; }
    public IPlayerService PlayerService { get; set; }
    public IWorldService WorldService { get; set; }

    public IEventBus EventBus { get; set; }
    public object Data { get; set; }

    public bool MoveRandomDirection() => MoveDirection(MoveDirectionType.East.RandomEnumValue());
    public bool MoveDirection(short direction) => MoveDirection((MoveDirectionType)direction);

    public bool MoveDirection(MoveDirectionType direction)
    {
        var newPosition = NpcGameObject.Position.ToPointPosition().AddMovement(direction);
        var canMove = WorldService.IsLocationWalkable(MapId, newPosition);
        if (canMove)
        {
            NpcGameObject.Position = newPosition.ToPoint();
            return true;
        }

        return false;
    }

    public bool MoveToPosition(int x, int y)
    {
        var newPosition = PointPosition.New(x, y);
        var canMove = WorldService.IsLocationWalkable(MapId, newPosition);
        if (canMove)
        {
            NpcGameObject.Position = newPosition.ToPoint();
            return true;
        }

        return false;
    }

    public bool MoveToPosition(PointPosition position) => MoveToPosition(position.X, position.Y);

    public bool SendNormalMessageAsync(string message) =>
        SendWorldMessageAsync(message, (short)WorldMessageType.Normal);

    public bool SendYellMessageAsync(string message) => SendWorldMessageAsync(message, (short)WorldMessageType.Yell);

    public bool SendWhisperMessageAsync(string message) =>
        SendWorldMessageAsync(message, (short)WorldMessageType.Whisper);

    public bool SendWorldMessageAsync(string message, short typeValue)
    {
        var type = (WorldMessageType)typeValue;
        return PlayerService.BroadcastChatMessageAsync(
                MapId,
                NpcGameObject.PointPosition(),
                NpcEntity.Name,
                NpcGameObject.ID,
                message,
                type
            )
            .GetAwaiter()
            .GetResult();
    }

    public void EnqueueObjectActionInCurrentLocation()
    {
        EventBus.Publish(
            new GameObjectCommandEvent
            {
                MapId = MapId,
                IsNpc = true,
                NpcId = NpcGameObject.ID,
                NpcObjectId = NpcEntity.Id,
                Position = NpcGameObject.PointPosition()
            }
        );
    }

    public List<BaseGameObject> GetEntitiesInRangeAsync(MapLayer layerValue, int range = 5)
    {
        var layer = (MapLayer)layerValue;
        var objects = WorldService.GetEntitiesInRangeAsync<BaseGameObject>(
                MapId,
                layer,
                NpcGameObject.Position.ToPointPosition(),
                range
            )
            .GetAwaiter()
            .GetResult();

        return objects;
    }

    public List<WorldGameObject> GetGameObjectsInRangeByName(short gameObjectType, int range = 5)
    {
        var objects = WorldService.GetEntitiesInRangeAsync<WorldGameObject>(
                MapId,
                MapLayer.Objects,
                NpcGameObject.Position.ToPointPosition(),
                range
            )
            .GetAwaiter()
            .GetResult();

        if (objects == null) return new();

        return objects.Where(s => s.Type == gameObjectType).ToList();
    }


    public void LogInfo(string text, params object[] args) => Logger.LogInformation(text, args);

    public List<PointPosition> CreateListOfPoints() => new();

    public List<PointPosition> GetPathToPosition(int x, int y) => WorldService.CalculateAStarPath(
        MapId,
        NpcGameObject.Position.ToPointPosition(),
        PointPosition.New(x, y)
    );

    public List<PointPosition> GetPathToPosition(PointPosition position) => GetPathToPosition(position.X, position.Y);
}
