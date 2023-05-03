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

[NetworkMessage(DarkStarMessageType.NpcRemovedResponse)]
[ProtoContract]
public struct NpcRemovedResponseMessage : IDarkStarNetworkMessage
{
    [ProtoMember(1)] public string MapId { get; set; } = null!;

    [ProtoMember(2)] public string NpcId { get; set; } = null!;


    public NpcRemovedResponseMessage()
    {
    }

    public NpcRemovedResponseMessage(string mapId, string npcId)
    {
        MapId = mapId;
        NpcId = npcId;
    }
}
