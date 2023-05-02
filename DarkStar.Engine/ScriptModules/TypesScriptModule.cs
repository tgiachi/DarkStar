using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.World.Types.GameObjects;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Engine.Attributes.ScriptEngine;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.ScriptModules;

[ScriptModule]
public class TypesScriptModule
{
    private readonly ILogger _logger;
    private readonly ITypeService _typeService;
    private readonly ISeedService _seedService;

    public TypesScriptModule(ILogger<TypesScriptModule> logger, ITypeService typeService, ISeedService seedService)
    {
        _logger = logger;
        _typeService = typeService;
        _seedService = seedService;
    }

    [ScriptFunction("add_game_object_type")]
    public GameObjectType AddGameObjectType(string type) => _typeService.AddGameObjectType(type);

    [ScriptFunction("add_npc_type")]
    public NpcSubType AddNpcType(string type, string subType) => _typeService.AddNpcSubType(type, subType);

    [ScriptFunction("add_tile")]
    public Tile AddTile(int id, string name, string category, string subcategory, string? tag, bool isTransparent)
    {
        _typeService.AddTile(new Tile(name, id, category, subcategory, isTransparent, tag));
        return _typeService.GetTile((uint)id);
    }

    [ScriptFunction("add_game_object_seed")]
    public void AddGameObject(string name, string description, string tileName, string gameObjectType, string data)
    {
        _seedService.AddGameObjectSeed(name, description, tileName, gameObjectType, data);
    }

    [ScriptFunction("add_item_type")]
    public void AddItemType(string name)
    {
        _typeService.AddItemType(name);
    }

    [ScriptFunction("add_item_category_type")]
    public void AddItemCategoryType(string name)
    {
        _typeService.AddItemCategoryType(name);
    }
}
