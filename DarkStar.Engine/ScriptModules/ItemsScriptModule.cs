using DarkStar.Api.Engine.Data.Items;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.ScriptModules;
using DarkStar.Engine.Attributes.ScriptEngine;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.ScriptModules;

[ScriptModule]
public class ItemsScriptModule : BaseScriptModule
{
    private readonly IItemService _itemService;
    private readonly ITypeService _typeService;

    public ItemsScriptModule(
        ILogger<ItemsScriptModule> logger, IDarkSunEngine engine, IItemService itemService, ITypeService typeService
    ) : base(logger, engine)
    {
        _itemService = itemService;
        _typeService = typeService;
    }

    [ScriptFunction("add_game_object_action", "Adds a game object action to the game object.")]
    public void AddScriptGameObject(string gameObjectType, Action<GameObjectContext> callBack)
    {
        _itemService.AddScriptableGameObject(_typeService.SearchGameObject(gameObjectType), callBack);
    }

    [ScriptFunction("add_scheduled_game_object_action", "Adds a scheduled game object action to the game object.")]
    public void AddScriptScheduledGameObject(string gameObjectType, int interval, Action<GameObjectContext> callBack)
    {
        _itemService.AddScriptableScheduledGameObject(_typeService.SearchGameObject(gameObjectType), interval, callBack);
    }
}
