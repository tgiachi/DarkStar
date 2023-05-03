using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Tiles;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Common;

[ProtoContract]
public class NamedMapEntityNetworkObject : MapEntityNetworkObject
{
    [ProtoMember(1)] public string Name { get; set; } = null!;

    public NamedMapEntityNetworkObject()
    {
    }

    public NamedMapEntityNetworkObject(int id, Guid objectId, uint tileType, PointPosition position, string name) : base(
        id,
        objectId,
        tileType,
        position
    ) => Name = name;
}
