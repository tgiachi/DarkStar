using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Triggers.Npc;


[ProtoContract]
[NetworkMessage(DarkStarMessageType.NpcMovedResponse)]
public struct NpcMovedResponseMessage : IDarkStarNetworkMessage
{
    [ProtoMember(1)]
    public string MapId { get; set; } = null!;
    [ProtoMember(2)]
    public string NpcId { get; set; } = null!;
    [ProtoMember(3)]
    public PointPosition Position { get; set; }

    public NpcMovedResponseMessage()
    {
    }

    public NpcMovedResponseMessage(string mapId, string npcId, PointPosition position)
    {
        MapId = mapId;
        NpcId = npcId;
        Position = position;
    }
}
