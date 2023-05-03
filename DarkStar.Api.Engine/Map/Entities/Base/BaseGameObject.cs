using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Utils;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Network.Protocol.Messages.Common;
using GoRogue;
using GoRogue.GameFramework;
using SadRogue.Primitives;

namespace DarkStar.Api.Engine.Map.Entities.Base;

public class BaseGameObject : GameObject
{
    public uint Tile { get; set; }
    public Guid ObjectId { get; set; }

    public BaseGameObject(Point position, int layer, bool isWalkable = true, bool isTransparent = true) : base(
        position,
        layer,
        isWalkable,
        isTransparent
    )
    {
    }

    public override string ToString() => $"{Tile} - ID: {ID} - ObjectId: {ObjectId}";

    public PointPosition PointPosition() => Position.ToPointPosition();
}
