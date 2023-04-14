using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Map;
using DarkStar.Network.Protocol.Messages.Common;
using Redbus.Events;

namespace DarkStar.Api.Engine.Events.Map
{
    public class GameObjectAddedEvent : EventBase
    {
        public string MapId { get; set; } = null!;
        public MapLayer Layer { get; set; }
        public PointPosition Position { get; set; }
        public Guid ObjectId { get; set; }

        public GameObjectAddedEvent(string mapId, MapLayer layer, PointPosition position, Guid objectId)
        {
            MapId = mapId;
            Layer = layer;
            Position = position;
            ObjectId = objectId;
        }

        public GameObjectAddedEvent()
        {

        }

        public override string ToString()
        {
            return $"GameObjectAddedEvent: {Layer} - {Position} - {ObjectId}";
        }
    }
}
