using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.World.Types.Map;
using DarkSun.Api.World.Types.Tiles;
using DarkSun.Network.Protocol.Messages.Common;

using ProtoBuf;

namespace DarkSun.Api.Engine.Serialization.Map
{
    [ProtoContract]
    public class LayerObjectSerialization
    {
        [ProtoMember(1)]
        public Guid ObjectId { get; set; }
        [ProtoMember(2)]
        public TileType Tile { get; set; }
        [ProtoMember(3)]
        public MapLayer Type { get; set; }
        [ProtoMember(4)]
        public PointPosition Position { get; set; }
    }
}
