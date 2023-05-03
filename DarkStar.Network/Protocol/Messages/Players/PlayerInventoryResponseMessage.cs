using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Players;

[NetworkMessage(DarkStarMessageType.PlayerInventoryResponse)]
[ProtoContract]
public class PlayerInventoryResponseMessage : IDarkStarNetworkMessage
{
    public List<PlayerInventoryItem> Items { get; set; } = new();
}

[ProtoContract]
public class PlayerInventoryItem
{
    [ProtoMember(1)] public Guid ItemId { get; set; }
    [ProtoMember(2)] public string ItemName { get; set; }
    [ProtoMember(3)] public string ItemDescription { get; set; }
    [ProtoMember(4)] public uint TileId { get; set; }
    [ProtoMember(5)] public int Quantity { get; set; }
}
