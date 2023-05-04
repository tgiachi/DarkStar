using DarkStar.Api.Engine.Interfaces.Services;
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
    public void AddGameObjectType(params string[] type)
    {
        type.ToList().ForEach(s => _typeService.AddGameObjectType(s));
    }

    [ScriptFunction("add_npc_type")]
    public NpcSubType AddNpcType(string type, string subType) => _typeService.AddNpcSubType(type, subType);

    [ScriptFunction("add_tile")]
    public Tile AddTile(int id, string name, string category, string subcategory, string? tag, bool isTransparent)
    {
        _typeService.AddTile(new Tile(name, id, category, subcategory, isTransparent, tag));
        return _typeService.GetTile((uint)id);
    }

    [ScriptFunction("add_game_object")]
    public void AddGameObject(string name, string description, string tileName, string gameObjectType, string data)
    {
        _seedService.AddGameObjectSeed(name, description, tileName, gameObjectType, data);
    }

    [ScriptFunction("add_item_type")]
    public void AddItemType(params string[] names)
    {
        names.ToList().ForEach(s => _typeService.AddItemType(s));
    }

    [ScriptFunction("add_item_category_type")]
    public void AddItemCategoryType(params string[] names)
    {
        names.ToList().ForEach(s => _typeService.AddItemCategoryType(s));
    }

    [ScriptFunction("add_text_content")]
    public void AddTextContent(string id, string content)
    {
        _seedService.AddTextContentSeed(id, content);
        _logger.LogDebug("Added text content seed {Id}", id.ToUpper());
    }


    [ScriptFunction("attach_text_content_to_item")]
    public void AttachTextContentToItem(string gameObjectName, string textContentId)
    {
        _seedService.AttachTextContentToItem(gameObjectName, textContentId);
        _logger.LogDebug("Attached text content {Id} to game object {GameObjectName}", textContentId.ToUpper(), gameObjectName.ToUpper());
    }

    [ScriptFunction("add_item")]
    public void AddItem(
        string name, string description, int weight, string tileName, string category, string type, short equipLocation,
        short itemRarity, string sellDice, string buyDice, string attackDice, string defenseDice, string speed
    )
    {
        var existType = _typeService.SearchItemType(type);
        var existCategory = _typeService.SearchItemCategoryType(category);

        if (string.IsNullOrEmpty(existType.Name))
        {
            _typeService.AddItemType(type);
        }

        if (string.IsNullOrEmpty(existCategory.Name))
        {
            _typeService.AddItemCategoryType(category);
        }

        _seedService.AddItemSeed(
            name,
            description,
            weight,
            tileName,
            category,
            type,
            equipLocation,
            itemRarity,
            sellDice,
            buyDice,
            attackDice,
            defenseDice,
            speed
        );
    }
}
