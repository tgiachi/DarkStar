using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Players;

[ProtoContract]
[NetworkMessage(DarkStarMessageType.PlayerGameObjectAddedResponse)]
public struct PlayerGameObjectAddedResponseMessage : IDarkStarNetworkMessage
{
    [ProtoMember(1)] public string MapId { get; set; }

    [ProtoMember(2)] public string Name { get; set; }

    [ProtoMember(3)] public string Id { get; set; }

    [ProtoMember(4)] public PointPosition Position { get; set; }

    [ProtoMember(5)] public int TileId { get; set; }

    public PlayerGameObjectAddedResponseMessage(string mapId, string name, string id, PointPosition position, int tileId)
    {
        MapId = mapId;
        Name = name;
        Id = id;
        Position = position;
        TileId = tileId;
    }

    public PlayerGameObjectAddedResponseMessage()
    {
    }
}
