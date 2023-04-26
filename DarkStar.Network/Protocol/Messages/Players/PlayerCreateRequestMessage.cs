using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;


using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Players;

[NetworkMessage(DarkStarMessageType.PlayerCreateRequest)]
[ProtoContract]
public class PlayerCreateRequestMessage : IDarkStarNetworkMessage
{
    [ProtoMember(1)]
    public string Name { get; set; } = null!;
    [ProtoMember(2)]
    public uint TileId { get; set; }
    [ProtoMember(3)]
    public Guid RaceId { get; set; }
    [ProtoMember(4)]
    public int Strength { get; set; }
    [ProtoMember(5)]
    public int Dexterity { get; set; }
    [ProtoMember(6)]
    public int Intelligence { get; set; }
    [ProtoMember(7)]
    public int Luck { get; set; }

}
