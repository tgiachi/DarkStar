using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Triggers.WorldObject;


[ProtoContract]
[NetworkMessage(DarkStarMessageType.WorldGameObjectAddedResponse)]
public struct WorldObjectAddedResponseMessage : IDarkStarNetworkMessage
{
    [ProtoMember(1)]
    public string MapId { get; set; } = null!;


    [ProtoMember(2)]
    public string ItemId { get; set; } = null!;
    [ProtoMember(3)]
    public string Name { get; set; } = null!;
    [ProtoMember(4)]
    public PointPosition Position { get; set; } = default;

    [ProtoMember(5)]
    public int TileType { get; set; } = 0;

    public WorldObjectAddedResponseMessage()
    {
    }

    public WorldObjectAddedResponseMessage(string mapId, string itemId, string name, PointPosition position, int tileType)
    {
        MapId = mapId;
        ItemId = itemId;
        Name = name;
        Position = position;
        TileType = tileType;
    }
}
