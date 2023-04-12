
using DarkSun.Api.World.Types.Tiles;
using DarkSun.Network.Attributes;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;
using ProtoBuf;


namespace DarkSun.Network.Protocol.Messages.Players
{
    [NetworkMessage(DarkSunMessageType.PlayerListResponse)]
    [ProtoContract]
    public class PlayerListResponseMessage : IDarkSunNetworkMessage
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
}
