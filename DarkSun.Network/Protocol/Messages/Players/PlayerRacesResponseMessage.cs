using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.World.Types.Tiles;
using DarkSun.Network.Attributes;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;

using ProtoBuf;

namespace DarkSun.Network.Protocol.Messages.Players
{
    [ProtoContract]
    [NetworkMessage(DarkSunMessageType.PlayerRacesResponse)]
    public class PlayerRacesResponseMessage : IDarkSunNetworkMessage
    {
        [ProtoMember(1)]
        public List<PlayerRaceObject> Races { get; set; } = null!;
    }
    [ProtoContract]
    public class PlayerRaceObject
    {
        [ProtoMember(1)]
        public Guid RaceId { get; set; }
        [ProtoMember(2)]
        public TileType TileId { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; } = null!;
        [ProtoMember(4)]
        public int Strength { get; set; }
        [ProtoMember(5)]
        public int Dexterity { get; set; }
        [ProtoMember(6)]
        public int Intelligence { get; set; }
        [ProtoMember(7)]
        public int Luck { get; set; }
    }
}
