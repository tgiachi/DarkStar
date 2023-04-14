using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Map.Entities.Base;
using DarkSun.Api.World.Types.Map;
using DarkSun.Network.Protocol.Messages.Common;

namespace DarkSun.Api.Engine.Events.Map
{
    public class GameObjectRemovedEvent : GameObjectAddedEvent
    {
        public GameObjectRemovedEvent(string mapId, MapLayer layer, PointPosition position, Guid objectId) : base(mapId, layer, position, objectId)
        {

        }


        public GameObjectRemovedEvent()
        {

        }

        public override string ToString()
        {
            return $"GameObjectRemovedEvent: {base.Layer} - {base.ObjectId}";
        }
    }
}
