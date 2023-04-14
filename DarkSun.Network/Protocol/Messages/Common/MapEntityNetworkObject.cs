using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.World.Types.Tiles;
using ProtoBuf;

namespace DarkSun.Network.Protocol.Messages.Common
{
    [ProtoContract]
    public class MapEntityNetworkObject
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public Guid ObjectId { get; set; }
        [ProtoMember(3)]
        public TileType TileType { get; set; }
        [ProtoMember(4)]
        public PointPosition Position { get; set; }
    }
}
