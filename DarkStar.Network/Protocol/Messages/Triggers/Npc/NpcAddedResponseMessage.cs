using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Network.Attributes;
using DarkStar.Network.Client.Interfaces;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Triggers.Npc;

[NetworkMessage(DarkStarMessageType.NpcAddedResponse)]
[ProtoContract]
public struct NpcAddedResponseMessage : IDarkStarNetworkMessage
{
    [ProtoMember(1)] public string MapId { get; set; } = null!;
    [ProtoMember(2)] public string NpcId { get; set; } = null!;
    [ProtoMember(3)] public string Name { get; set; } = null!;
    [ProtoMember(4)] public PointPosition Position { get; set; }
    [ProtoMember(5)] public int TileType { get; set; }

    public NpcAddedResponseMessage()
    {
    }

    public NpcAddedResponseMessage(string mapId, string npcId, string name, PointPosition position, int tileType)
    {
        MapId = mapId;
        NpcId = npcId;
        Name = name;
        Position = position;
        TileType = tileType;
    }
}
