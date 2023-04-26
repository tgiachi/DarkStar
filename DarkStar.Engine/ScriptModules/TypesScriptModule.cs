using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.World.Types.GameObjects;
using DarkStar.Engine.Attributes.ScriptEngine;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.ScriptModules;

[ScriptModule]
public class TypesScriptModule
{
    private readonly ILogger _logger;
    private readonly ITypeService _typeService;

    public TypesScriptModule(ILogger<TypesScriptModule> logger, ITypeService typeService)
    {
        _logger = logger;
        _typeService = typeService;
    }


    [ScriptFunction("add_game_object_type")]
    public GameObjectType AddGameObjectType(string type) => _typeService.AddGameObjectType(type);
}
