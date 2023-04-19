using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Protocol.Messages.Common;
using SadRogue.Primitives;

namespace DarkStar.Api.Engine.Utils
{
    public static class PointConverterEx
    {
        public static PointPosition ToPointPosition(this Point point) => new(point.X, point.Y);
        public static Point ToPoint(this PointPosition pointPosition) => new(pointPosition.X, pointPosition.Y);

        public static PointPosition AddMovement(this PointPosition position, PlayerMoveDirectionType direction)
        {
            return direction switch
            {
                PlayerMoveDirectionType.North => new PointPosition(position.X, position.Y - 1),
                PlayerMoveDirectionType.South => new PointPosition(position.X, position.Y + 1),
                PlayerMoveDirectionType.East => new PointPosition(position.X + 1, position.Y),
                PlayerMoveDirectionType.West => new PointPosition(position.X - 1, position.Y),
                PlayerMoveDirectionType.NorthEast => new PointPosition(position.X + 1, position.Y - 1),
                PlayerMoveDirectionType.NorthWest => new PointPosition(position.X - 1, position.Y - 1),
                PlayerMoveDirectionType.SouthEast => new PointPosition(position.X + 1, position.Y + 1),
                PlayerMoveDirectionType.SouthWest => new PointPosition(position.X - 1, position.Y + 1),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }


}
