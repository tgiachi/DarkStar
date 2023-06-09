using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Map;
using DarkStar.Network.Protocol.Messages.Common;
using Redbus.Events;

namespace DarkStar.Api.Engine.Events.Map;

public class GameObjectAddedEvent : EventBase
{
    public string MapId { get; set; } = null!;
    public MapLayer Layer { get; set; }
    public PointPosition Position { get; set; }
    public Guid ObjectId { get; set; }

    public uint Id { get; set; }

    public GameObjectAddedEvent(string mapId, MapLayer layer, PointPosition position, Guid objectId, uint id)
    {
        MapId = mapId;
        Layer = layer;
        Position = position;
        ObjectId = objectId;
        Id = id;
    }

    public GameObjectAddedEvent()
    {
    }

    public override string ToString() => $"GameObjectAddedEvent: {Layer} - {Position} - {ObjectId}";
}
