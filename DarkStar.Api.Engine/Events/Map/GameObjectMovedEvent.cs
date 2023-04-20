using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Map;
using DarkStar.Network.Protocol.Messages.Common;

namespace DarkStar.Api.Engine.Events.Map;

public class GameObjectMovedEvent : GameObjectAddedEvent
{
    public PointPosition OldPosition { get; set; }

    public GameObjectMovedEvent(string mapId, MapLayer layer, PointPosition oldPosition, PointPosition newPosition, Guid objectId) : base(
       mapId, layer, newPosition, objectId)
    {
        OldPosition = oldPosition;
    }

    public GameObjectMovedEvent()
    {

    }

    public override string ToString()
    {
        return $"GameObjectMovedEvent: {Layer} - {ObjectId} - {OldPosition} -> {Position}";
    }
}
