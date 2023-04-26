using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.World.Types.GameObjects;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Engine.Services.Base;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Services;

[DarkStarEngineService(nameof(TypeService), 1)]
public class TypeService : BaseService<TypeService>, ITypeService
{
    public List<Tile> Tiles => _tiles;
    public List<GameObjectType> GameObjectTypes => _gameObjectTypes;


    private readonly Dictionary<uint, Tile> _tilesById = new();
    private readonly Dictionary<string, Tile> _tilesByName = new();
    private readonly List<Tile> _tiles = new();

    private readonly List<GameObjectType> _gameObjectTypes = new();
    private readonly Dictionary<ushort, string> _gameObjectTypesById = new();
    private readonly Dictionary<ushort, string> _aiBehaviour = new();
    private readonly Dictionary<ushort, string> _npcTypes = new();
    private readonly Dictionary<ushort, string> _itemTypes = new();



    public Tile GetTile(uint id) => _tilesById[id];
    public Tile GetTile(string name) => _tilesByName[name.ToLower()];


    public TypeService(ILogger<TypeService> logger) : base(logger)
    {


    }


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




    public void AddTile(Tile tile)
    {
        Logger.LogInformation("Adding tile: {Id} - {Name}", tile.Id, tile);
        _tilesByName.Add(tile.FullName.ToLower(), tile);
        _tilesById.Add(tile.Id, tile);
        _tiles.Add(tile);
    }

    public GameObjectType AddGameObjectType(string name)
    {
        var id = (ushort)_gameObjectTypes.Count;
        _gameObjectTypes.Add(new GameObjectType(id, name));
        _gameObjectTypesById.Add(id, name);

        return _gameObjectTypes.Last();
    }

    public GameObjectType AddGameObjectType(ushort id, string name)
    {
        if (id == -1)
        {
            return AddGameObjectType(name);
        }

        _gameObjectTypes.Add(new GameObjectType(id, name));
        _gameObjectTypesById.Add(id, name);
        return _gameObjectTypes.Last();
    }

    public GameObjectType GetGameObjectType(string name) => _gameObjectTypes.First(t => t.Name == name);

    public GameObjectType GetGameObjectType(ushort id) => _gameObjectTypes.First(t => t.Id == id);
}
