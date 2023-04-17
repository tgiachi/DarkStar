using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.TileSet
{

    [ProtoContract]
    [NetworkMessage(DarkStarMessageType.TileSetDownloadResponse)]
    public struct TileSetDownloadResponseMessage : IDarkStarNetworkMessage
    {
        [ProtoMember(1)] public string TileSetName { get; set; } = null!;

        [ProtoMember(2)]
        public byte[] TileSetData { get; set; } = null!;


        public TileSetDownloadResponseMessage(string name, byte[] tileSetData)
        {
            TileSetName = name;
            TileSetData = tileSetData;
        }

        public TileSetDownloadResponseMessage()
        {
        }
    }
}
