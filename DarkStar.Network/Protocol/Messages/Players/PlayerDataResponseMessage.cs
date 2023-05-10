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

namespace DarkStar.Network.Protocol.Messages.Players;

[NetworkMessage(DarkStarMessageType.PlayerDataResponse)]
[ProtoContract]
public struct PlayerDataResponseMessage : IDarkStarNetworkMessage
{
    public string MapId { get; set; }

    public int Level { get; set; }
    public uint TileId { get; set; }

    public PointPosition Position { get; set; }

    public Guid PlayerId { get; set; }

    public string Name { get; set; }

    public PlayerDataResponseMessage(string mapId, uint tileId, PointPosition position, Guid playerId, string name, int level)
    {
        Level = level;
        Name = name;
        MapId = mapId;
        TileId = tileId;
        Position = position;
        PlayerId = playerId;
    }

    public PlayerDataResponseMessage()
    {
    }
}
