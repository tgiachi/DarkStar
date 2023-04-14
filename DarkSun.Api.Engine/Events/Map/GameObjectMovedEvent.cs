using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.World.Types.Map;
using DarkSun.Network.Protocol.Messages.Common;

namespace DarkSun.Api.Engine.Events.Map
{
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
}
