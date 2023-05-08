using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.TileSet;

[ProtoContract]
[NetworkMessage(DarkStarMessageType.TileSetListResponse)]
public class TileSetListResponseMessage : IDarkStarNetworkMessage
{
    public List<TileSetEntryMessage> TileSets { get; set; } = null!;
}

[ProtoContract]
public class TileSetEntryMessage
{
    [ProtoMember(1)] public string Id { get; set; } = null!;
    [ProtoMember(2)] public string Name { get; set; } = null!;
    [ProtoMember(3)] public int TileHeight { get; set; }
    [ProtoMember(4)] public int TileWidth { get; set; }
    [ProtoMember(5)] public long FileSize { get; set; }
}
