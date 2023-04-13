using DarkSun.Api.World.Types.Map;
using ProtoBuf;

namespace DarkSun.Api.Engine.Serialization.Map
{
    [ProtoContract]
    public class MapObjectSerialization
    {
        [ProtoMember(1)]
        public Guid MapId { get; set; }

        [ProtoMember(2)]
        public MapType MapType { get; set; }

        [ProtoMember(3)]
        public string Name { get; set; } = null!;
        [ProtoMember(4)] public List<LayerObjectSerialization> Layers { get; set; } = new();
    }
}
