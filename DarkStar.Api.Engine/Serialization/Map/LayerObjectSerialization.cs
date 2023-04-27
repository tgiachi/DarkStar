using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Map;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Network.Protocol.Messages.Common;

using ProtoBuf;

namespace DarkStar.Api.Engine.Serialization.Map;

[ProtoContract]
public class LayerObjectSerialization
{
    [ProtoMember(1)]
    public Guid ObjectId { get; set; }
    [ProtoMember(2)]
    public uint Tile { get; set; }
    [ProtoMember(3)]
    public MapLayer Type { get; set; }
    [ProtoMember(4)]
    public PointPosition Position { get; set; }

    [ProtoMember(5)]
    public Dictionary<string, string> Properties { get; set; } = new();
}
