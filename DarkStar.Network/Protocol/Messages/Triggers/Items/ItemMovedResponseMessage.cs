using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Triggers.Items;


[ProtoContract]
[NetworkMessage(DarkStarMessageType.ItemMovedResponse)]
public struct ItemMovedResponseMessage : IDarkStarNetworkMessage
{
    [ProtoMember(1)]
    public string MapId { get; set; } = null!;
    [ProtoMember(2)]
    public string ItemId { get; set; } = null!;
    [ProtoMember(3)]
    public PointPosition Position { get; set; }

    public ItemMovedResponseMessage()
    {
    }

    public ItemMovedResponseMessage(string mapId, string itemId, PointPosition position)
    {
        MapId = mapId;
        ItemId = itemId;
        Position = position;
    }
}
