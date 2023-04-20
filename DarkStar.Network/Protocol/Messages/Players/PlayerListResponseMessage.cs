using DarkStar.Api.World.Types.Tiles;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;

using ProtoBuf;


namespace DarkStar.Network.Protocol.Messages.Players;

[NetworkMessage(DarkStarMessageType.PlayerListResponse)]
[ProtoContract]
public class PlayerListResponseMessage : IDarkStarNetworkMessage
{
    [ProtoMember(1)]
    public List<PlayerObjectMessage> Players { get; set; } = null!;
    public PlayerListResponseMessage() { }

    public PlayerListResponseMessage(List<PlayerObjectMessage> players)
    {
        Players = players;
    }
}

[ProtoContract]
public class PlayerObjectMessage
{
    [ProtoMember(1)]
    public Guid Id { get; set; }
    [ProtoMember(2)]
    public string Name { get; set; } = null!;
    [ProtoMember(3)]
    public TileType Tile { get; set; }
    [ProtoMember(4)]
    public int Level { get; set; }
    [ProtoMember(5)]
    public string Race { get; set; } = null!;
}
