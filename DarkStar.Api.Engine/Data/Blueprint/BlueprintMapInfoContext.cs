using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.World.Types.Map;
using DarkStar.Api.World.Types.Tiles;

namespace DarkStar.Api.Engine.Data.Blueprint;

public class BlueprintMapInfoContext
{

    private readonly ITypeService _typeService;

    public BlueprintMapInfoContext(ITypeService typeService) => _typeService = typeService;

    public MapType MapType { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int MapStrategy { get; set; }

    public Tile BlockingTile { get; set; }

    public Tile NonBlockingTile { get; set; }


    public void SetMapSize(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public void SetMapStrategy(int strategy)
    {
        MapStrategy = strategy;
    }

    public void SetTerrainTiles(string blockingTile, string nonBlockingTile)
    {
        BlockingTile = _typeService.SearchTile(blockingTile, null, null);
        NonBlockingTile =  _typeService.SearchTile(nonBlockingTile, null, null);
    }
}
