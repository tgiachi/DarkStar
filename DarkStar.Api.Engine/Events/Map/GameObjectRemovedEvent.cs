using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Map;
using DarkStar.Network.Protocol.Messages.Common;


namespace DarkStar.Api.Engine.Events.Map;

public class GameObjectRemovedEvent : GameObjectAddedEvent
{
    public GameObjectRemovedEvent(string mapId, MapLayer layer, PointPosition position, Guid objectId, uint id) : base(mapId, layer, position, objectId, id)
    {

    }


    public GameObjectRemovedEvent()
    {

    }

    public override string ToString()
    {
        return $"GameObjectRemovedEvent: {Layer} - {ObjectId}";
    }
}
