using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Protocol.Messages.Common;
using SadRogue.Primitives;

namespace DarkStar.Api.Engine.Utils;

public static class PointConverterEx
{
    public static PointPosition ToPointPosition(this Point point) => new(point.X, point.Y);
    public static Point ToPoint(this PointPosition pointPosition) => new(pointPosition.X, pointPosition.Y);

    public static PointPosition AddMovement(this PointPosition position, MoveDirectionType direction)
    {
        return direction switch
        {
            MoveDirectionType.North     => new PointPosition(position.X, position.Y - 1),
            MoveDirectionType.South     => new PointPosition(position.X, position.Y + 1),
            MoveDirectionType.East      => new PointPosition(position.X + 1, position.Y),
            MoveDirectionType.West      => new PointPosition(position.X - 1, position.Y),
            MoveDirectionType.NorthEast => new PointPosition(position.X + 1, position.Y - 1),
            MoveDirectionType.NorthWest => new PointPosition(position.X - 1, position.Y - 1),
            MoveDirectionType.SouthEast => new PointPosition(position.X + 1, position.Y + 1),
            MoveDirectionType.SouthWest => new PointPosition(position.X - 1, position.Y + 1),
            _                           => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}
