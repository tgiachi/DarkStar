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

namespace DarkStar.Network.Protocol.Messages.TileSet
{
    [ProtoContract]
    [NetworkMessage(DarkStarMessageType.TileSetMapResponse)]
    public struct TileSetMapResponseMessage : IDarkStarNetworkMessage
    {
        [ProtoMember(1)] public string TileSetName { get; set; } = null!;

        [ProtoMember(2)]
        public List<TileSetMapEntry> TileSetMap { get; set; } = new();

        public TileSetMapResponseMessage(string tileSetName, List<TileSetMapEntry> tileSetMap)
        {
            TileSetName = tileSetName;
            TileSetMap = tileSetMap;
        }

        public TileSetMapResponseMessage()
        {
        }
    }

    [ProtoContract]
    public struct TileSetMapEntry
    {
        [ProtoMember(1)]
        public TileType TileType { get; set; }

        [ProtoMember(2)]
        public int Id { get; set; }

        public TileSetMapEntry()
        {

        }

        public TileSetMapEntry(TileType tileType, int id)
        {
            TileType = tileType;
            Id = id;
        }
    }
}
