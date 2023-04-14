using DarkSun.Api.World.Types.Map;
using DarkSun.Network.Attributes;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Messages.Common;
using DarkSun.Network.Protocol.Types;
using ProtoBuf;

namespace DarkSun.Network.Protocol.Map
{
    [ProtoContract]
    [NetworkMessage(DarkSunMessageType.MapResponse)]
    public class MapResponseMessage : IDarkSunNetworkMessage
    {
        [ProtoMember(1)]
        public string MapId { get; set; } = null!;

        [ProtoMember(2)]
        public string Name { get; set; } = null!;

        [ProtoMember(3)]
        public MapType MapType { get; set; }

        [ProtoMember(4)]
        public int Width { get; set; }
        [ProtoMember(5)]
        public int Height { get; set; }

        [ProtoMember(6)]
        public List<MapEntityNetworkObject> TerrainsLayer { get; set; } = new();

        [ProtoMember(7)]
        public List<MapEntityNetworkObject> GameObjectsLayer { get; set; } = new();

        [ProtoMember(8)]
        public List<NamedMapEntityNetworkObject> NpcsLayer { get; set; } = new();

        [ProtoMember(9)]
        public List<NamedMapEntityNetworkObject> ItemsLayer { get; set; } = new();

        [ProtoMember(10)]
        public List<NamedMapEntityNetworkObject> PlayersLayer { get; set; } = new();

    }
}
