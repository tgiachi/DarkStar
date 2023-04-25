using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Engine.Services.Base;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Services;

[DarkStarEngineService(nameof(TileService), 1)]
public class TileService : BaseService<TileService>, ITileService
{
    public List<Tile> Tiles  => _tiles;
    private readonly Dictionary<uint, Tile> _tilesById = new();
    private readonly Dictionary<string, Tile> _tilesByName = new();
    private readonly List<Tile> _tiles = new();
    public Tile GetTile(uint id) => _tilesById[id];
    public Tile GetTile(string name) => _tilesByName[name.ToLower()];

    public List<Tile> SearchTiles(string name, string? category, string? subCategory)
    {
        var tiles = _tiles.Where(t => t.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        if (category != null)
        {
            tiles = tiles.Where(t => t.Category.Contains(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (subCategory != null)
        {
            tiles = tiles.Where(t => t.SubCategory.Contains(subCategory, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        return tiles;

    }

  

    public TileService(ILogger<TileService> logger) : base(logger)
    {


    }

    public void AddTile(Tile tile)
    {
        Logger.LogInformation("Adding tile: {Id} - {Name}", tile.Id, tile);
        _tilesByName.Add(tile.FullName.ToLower(), tile);
        _tilesById.Add(tile.Id, tile);
        _tiles.Add(tile);
    }

}
