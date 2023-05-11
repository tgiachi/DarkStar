using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Data.Blueprint;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.World.Types.Map;
using DarkStar.Engine.Attributes.ScriptEngine;
using FastEnumUtility;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.ScriptModules;

[ScriptModule]
public class BlueprintScriptModule
{
    private readonly IBlueprintService _blueprintService;
    private readonly ILogger _logger;

    public BlueprintScriptModule(ILogger<BlueprintScriptModule> logger, IBlueprintService blueprintService)
    {
        _blueprintService = blueprintService;
        _logger = logger;
    }

    [ScriptFunction("add_map_generator")]
    public void AddMapGenerator(short mapType, Func<BlueprintGenerationMapContext, BlueprintGenerationMapContext> callBack)
    {
        var mapTypeEnum = FastEnum.Parse<MapType>(mapType.ToString());
        _logger.LogDebug("Adding map generator for mapType: {MapType}", mapTypeEnum);
        _blueprintService.AddMapGenerator(mapTypeEnum, callBack);
    }

    [ScriptFunction("add_city_map_generator")]
    public void AddCityMapGenerator(Func<BlueprintGenerationMapContext, BlueprintGenerationMapContext> callBack)
    {
        AddMapGenerator((short)MapType.City, callBack);
    }

    [ScriptFunction("add_dungeon_map_generator")]
    public void AddDungeonMapGenerator(Func<BlueprintGenerationMapContext, BlueprintGenerationMapContext> callBack)
    {
        AddMapGenerator((short)MapType.Dungeon, callBack);
    }

    [ScriptFunction("add_map_strategy")]
    public void AddMapCreationStrategy(short mapType, Func<BlueprintMapInfoContext, BlueprintMapInfoContext> callBack)
    {
        var mapTypeEnum = FastEnum.Parse<MapType>(mapType.ToString());
        _logger.LogDebug("Adding map creation strategy for mapType: {MapType}", mapTypeEnum);
        _blueprintService.AddMapStrategy(mapTypeEnum, callBack);
    }
}
