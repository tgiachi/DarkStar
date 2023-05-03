using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Engine.Events.Engine;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Utils;
using DarkStar.Api.World.Types.GameObjects;
using DarkStar.Api.World.Types.Items;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Engine.Services.Base;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Services;

[DarkStarEngineService(nameof(TypeService), 1)]
public class TypeService : BaseService<TypeService>, ITypeService
{
    public List<Tile> Tiles => _tiles;
    public List<NpcType> NpcTypes => _npcTypes;
    public List<NpcSubType> NpcSubTypes => _npcSubTypes;
    public List<GameObjectType> GameObjectTypes => _gameObjectTypes;

    private readonly Dictionary<uint, Tile> _tilesById = new();
    private readonly Dictionary<string, Tile> _tilesByName = new();
    private readonly List<Tile> _tiles = new();
    private readonly List<NpcType> _npcTypes = new();
    private readonly List<NpcSubType> _npcSubTypes = new();
    private readonly List<ItemType> _itemTypes = new();
    private readonly List<ItemCategoryType> _itemCategoryTypes = new();

    private readonly List<GameObjectType> _gameObjectTypes = new();
    private readonly Dictionary<short, string> _gameObjectTypesById = new();
    private readonly Dictionary<short, string> _aiBehaviour = new();
    private readonly Dictionary<short, string> _npcTypesById = new();

    private readonly Dictionary<short, string> _npcSubTypesById = new();
    private readonly Dictionary<short, string> _itemTypesById = new();

    private readonly List<(NpcType, NpcSubType, string)> _npcTypeTiles = new();

    public Tile GetTile(uint id) => _tilesById[id];
    public Tile GetTile(string name) => _tilesByName[name.ToLower()];


    public TypeService(ILogger<TypeService> logger) : base(logger)
    {
    }

    public List<Tile> SearchTiles(string name, string? category, string? subCategory)
    {
        var tiles = _tiles.Where(t => SearchListUtils.MatchesWildcard(t.FullName, name)).ToList();
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

    public Tile SearchTile(string name, string? category, string? subCategory)
    {
        bool result = int.TryParse(name, out var i);
        if (result)
        {
            return new Tile("Unknown", i, "Unknown", "Unknown", false, null);
        }

        var results = SearchTiles(name, category, subCategory);
        return results.Count > 1 ? results.RandomItem() : results.First();
    }

    public void AddTile(Tile tile)
    {
        Logger.LogInformation("Adding tile: {Id} - {Name}", tile.Id, tile);
        _tilesByName.Add(tile.FullName.ToLower(), tile);
        if (_tilesById.ContainsKey(tile.Id))
        {
            Logger.LogWarning("Tile Id {Id} already exists!", tile.Id);
        }
        else
        {
            _tilesById.Add(tile.Id, tile);
        }

        Engine.EventBus.PublishAsync(new TileAddedEvent() { Tile = tile });
        //_tilesById.Add(tile.Id, tile);
        _tiles.Add(tile);
    }

    public GameObjectType AddGameObjectType(string name)
    {
        var id = (short)_gameObjectTypes.Count;
        _gameObjectTypes.Add(new GameObjectType(id, name));
        _gameObjectTypesById.Add(id, name);

        var go = _gameObjectTypes.Last();
        Engine.EventBus.PublishAsync(new GameObjectTypeAdded() { GameObjectType = go });
        return go;
    }

    public GameObjectType AddGameObjectType(short id, string name)
    {
        if (id == -1)
        {
            return AddGameObjectType(name);
        }

        _gameObjectTypes.Add(new GameObjectType(id, name));
        _gameObjectTypesById.Add(id, name);
        return _gameObjectTypes.Last();
    }

    public GameObjectType GetGameObjectType(string name) => _gameObjectTypes.First(t => t.Name == name.ToUpper());

    public GameObjectType GetGameObjectType(short id) => _gameObjectTypes.First(t => t.Id == id);

    public GameObjectType SearchGameObject(string name) =>
        _gameObjectTypes.First(t => SearchListUtils.MatchesWildcard(t.Name, name));

    public NpcType AddNpcType(string name)
    {
        var id = (short)_npcTypes.Count;
        _npcTypesById.Add(id, name);
        _npcTypes.Add(new NpcType(id, name));
        var npc = _npcTypes.Last();
        Engine.EventBus.PublishAsync(new NpcTypeAdded() { NpcType = npc });
        return npc;
    }

    public NpcType AddNpcType(short id, string name)
    {
        if (id == -1)
        {
            return AddNpcType(name);
        }

        _npcTypesById.Add(id, name);
        _npcTypes.Add(new NpcType(id, name));
        return _npcTypes.Last();
    }

    public NpcSubType AddNpcSubType(string npcType, string name)
    {
        var type = GetNpcType(npcType);
        if (type.Value.Name == null)
        {
            AddNpcType(npcType);
        }

        type = GetNpcType(npcType);
        var id = (short)_npcSubTypes.Count;
        _npcSubTypesById.Add(id, name);
        _npcSubTypes.Add(new NpcSubType(type.Value.Id, id, name));
        var subType = _npcSubTypes.Last();
        Engine.EventBus.PublishAsync(new NpcSubTypeAdded() { NpcSubType = subType });
        return subType;
    }

    public NpcSubType AddNpcSubType(string npcType, short id, string name)
    {
        if (id == -1)
        {
            return AddNpcSubType(npcType, name);
        }

        var type = GetNpcType(npcType);
        if (type == null)
        {
            AddNpcType(npcType);
        }

        _npcSubTypesById.Add(id, name);
        _npcSubTypes.Add(new NpcSubType(type.Value.Id, id, name));
        return _npcSubTypes.Last();
    }

    public NpcSubType GetNpcSubType(string name) => _npcSubTypes.First(t => t.Name.ToUpper() == name.ToUpper());
    public NpcSubType GetNpcSubType(short id) => _npcSubTypes.First(t => t.Id == id);

    public NpcType? GetNpcType(string name) => _npcTypes.FirstOrDefault(t => t.Name.ToUpper() == name.ToUpper());
    public NpcType GetNpcType(short id) => _npcTypes.FirstOrDefault(t => t.Id == id);

    public void AddNpcTypeTile(NpcType npcType, NpcSubType npcSubType, string tile)
    {
        _npcTypeTiles.Add((npcType, npcSubType, tile));
    }

    public Tile GetTileForNpc(NpcType npcType, NpcSubType npcSubType)
    {
        var tileId = _npcTypeTiles.First(t => t.Item1.Id == npcType.Id && t.Item2.Id == npcSubType.Id).Item3;

        return SearchTile(tileId, null, null);
    }

    public Tile GetTileForNpc(string npcType, string npcSubType)
    {
        var tileId = _npcTypeTiles.First(
            t => t.Item1.Name.ToUpper() == npcType.ToUpper() && t.Item2.Name.ToUpper() == npcSubType.ToUpper()
        );
        return SearchTile(tileId.Item3, null, null);
    }

    public ItemType AddItemType(string name)
    {
        var exists = _itemTypes.FirstOrDefault(s => s.Name == name.ToUpper());
        if (!string.IsNullOrEmpty(exists.Name))
        {
            return exists;
        }

        _itemTypes.Add(
            new ItemType
            {
                Id = (ushort)_itemTypes.Count,
                Name = name.ToUpper()
            }
        );

        return _itemTypes.Last();
    }

    public ItemType AddItemType(short id, string name)
    {
        _itemTypes.Add(new ItemType((ushort)id, name.ToUpper()));
        return _itemTypes.Last();
    }

    public ItemType SearchItemType(string name)
    {
        return _itemTypes.Where(s => SearchListUtils.MatchesWildcard(s.Name, name)).ToList().RandomItem();
    }

    public ItemCategoryType AddItemCategoryType(string name)
    {
        var exists = _itemCategoryTypes.FirstOrDefault(s => s.Name == name.ToUpper());
        if (!string.IsNullOrEmpty(exists.Name))
        {
            return exists;
        }

        _itemCategoryTypes.Add(
            new ItemCategoryType
            {
                Id = (ushort)_itemCategoryTypes.Count,
                Name = name.ToUpper()
            }
        );

        return _itemCategoryTypes.Last();
    }

    public ItemCategoryType AddItemCategoryType(short id, string name)
    {
        _itemCategoryTypes.Add(
            new ItemCategoryType
            {
                Id = (ushort)id,
                Name = name.ToUpper()
            }
        );

        return _itemCategoryTypes.Last();
    }

    public ItemCategoryType SearchItemCategoryType(string name)
    {
        return _itemCategoryTypes.Where(s => SearchListUtils.MatchesWildcard(s.Name, name)).ToList().RandomItem();
    }
}
