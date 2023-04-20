using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Tiles;

namespace DarkStar.Api.Engine.Data.Templates;

public class BluePrintTemplatePoint
{
    public TileType TileType { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public BluePrintTemplatePoint(TileType tileType, int x, int y)
    {
        TileType = tileType;
        X = x;
        Y = y;
    }

    public BluePrintTemplatePoint()
    {
    }

    public override string ToString() => $"TileType: {TileType}, X: {X}, Y: {Y}";
}
