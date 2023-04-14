using DarkSun.Api.World.Types.Map;
using ProtoBuf;

namespace DarkSun.Api.Engine.Serialization.Map
{
    [ProtoContract]
    public class MapObjectSerialization
    {
        [ProtoMember(1)] public string MapId { get; set; } = null!;

        [ProtoMember(2)]
        public MapType MapType { get; set; }

        [ProtoMember(3)]
        public string Name { get; set; } = null!;

        [ProtoMember(4)]
        public int Width { get; set; }
        [ProtoMember(5)]
        public int Height { get; set; }

        [ProtoMember(6)] public List<LayerObjectSerialization> Layers { get; set; } = new();
    }
}
