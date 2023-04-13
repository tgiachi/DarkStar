using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.World.Types.Tiles;

namespace DarkSun.Api.World.Types.Utils
{
    public static class TilesEnumEx
    {
        public static TileType ParseTileType(this short tileId)
        {
            return Enum.IsDefined(typeof(TileType), tileId) ? (TileType)tileId : TileType.Null;
        }
    }
}
