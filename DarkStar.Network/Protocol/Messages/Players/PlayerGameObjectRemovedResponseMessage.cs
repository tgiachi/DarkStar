using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Players;

[NetworkMessage(DarkStarMessageType.PlayerGameObjectRemovedResponse)]
[ProtoContract]
public class PlayerGameObjectRemovedResponseMessage : IDarkStarNetworkMessage
{
    public string MapId { get; set; }

    public string Id { get; set; }


    public PlayerGameObjectRemovedResponseMessage(string mapId, string id)
    {
        MapId = mapId;
        Id = id;
    }

    public PlayerGameObjectRemovedResponseMessage()
    {
    }
}
