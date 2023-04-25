using DarkStar.Api.World.Types.Tiles;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Triggers.Items;


[NetworkMessage(DarkStarMessageType.ItemAddedResponse)]
[ProtoContract]
public struct ItemAddedResponseMessage : IDarkStarNetworkMessage
{
    [ProtoMember(1)] public string MapId { get; set; } = null!;

    [ProtoMember(2)]
    public string ItemId { get; set; } = null!;
    [ProtoMember(3)]
    public string Name { get; set; } = null!;

    [ProtoMember(4)]
    public PointPosition Position { get; set; }

    [ProtoMember(5)]
    public int TileType { get; set; }

    public ItemAddedResponseMessage()
    {

    }

    public ItemAddedResponseMessage(string mapId, string itemId, string name, PointPosition position, int tileType)
    {
        MapId = mapId;
        ItemId = itemId;
        Name = name;
        Position = position;
        TileType = tileType;
    }
}
