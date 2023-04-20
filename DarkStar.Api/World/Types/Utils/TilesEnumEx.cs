using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Tiles;
using FastEnumUtility;

namespace DarkStar.Api.World.Types.Utils;

public static class TilesEnumEx
{
    public static TileType ParseTileType(this short tileId)
    {
        return FastEnum.IsDefined<TileType>(tileId) ? (TileType)tileId : TileType.Null;
    }
}
