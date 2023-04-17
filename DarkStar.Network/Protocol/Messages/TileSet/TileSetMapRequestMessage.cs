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
    [NetworkMessage(DarkStarMessageType.TileSetMapRequest)]
    [ProtoContract]
    public class TileSetMapRequestMessage : IDarkStarNetworkMessage
    {
        [ProtoMember(1)] public string TileSetName { get; set; } = null!;

        public TileSetMapRequestMessage(string tileSetName)
        {
            TileSetName = tileSetName;
        }

        public TileSetMapRequestMessage()
        {
        }

    }


}
