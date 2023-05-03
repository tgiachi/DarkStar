using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Triggers.WorldObject;

[NetworkMessage(DarkStarMessageType.WorldGameObjectRemovedResponse)]
[ProtoContract]
public struct WorldObjectRemovedResponseMessage : IDarkStarNetworkMessage
{
    [ProtoMember(1)] public string MapId { get; set; } = null!;

    [ProtoMember(2)] public string ItemId { get; set; } = null!;


    public WorldObjectRemovedResponseMessage()
    {
    }

    public WorldObjectRemovedResponseMessage(string mapId, string itemId)
    {
        MapId = mapId;
        ItemId = itemId;
    }
}
