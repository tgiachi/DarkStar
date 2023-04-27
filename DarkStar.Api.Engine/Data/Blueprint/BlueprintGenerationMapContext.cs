using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.Map.Entities;

namespace DarkStar.Api.Engine.Data.Blueprint;
public class BlueprintGenerationMapContext
{
    private readonly IBlueprintService _blueprintService;
    private readonly ITypeService _typeService;
    private readonly INamesService _namesService;
    private readonly IWorldService _worldService;
    private readonly string _mapId;

    private readonly List<WorldGameObject> _gameObjects = new();

    public List<WorldGameObject> GameObjects => _gameObjects;
    protected IBlueprintService BlueprintService => _blueprintService;
    protected ITypeService TypeService => _typeService;
    protected INamesService NamesService => _namesService;

    protected IWorldService WorldService => _worldService;

    public BlueprintGenerationMapContext(string mapId, IDarkSunEngine engine)
    {
        _mapId = mapId;
        _blueprintService = engine.BlueprintService;
        _typeService = engine.TypeService;
        _namesService = engine.NamesService;
        _worldService = engine.WorldService;
    }

    public void AddGameObject(short gameObjectId)
    {
        _ = BlueprintService.GenerateWorldGameObjectAsync(
                _typeService.GetGameObjectType(gameObjectId),
                WorldService.GetRandomWalkablePosition(_mapId)
            )
            .ContinueWith(
                task =>
                {
                    _gameObjects.Add(task.Result);

                }, TaskScheduler.Current);
    }

    public void AddGameObjects(int count, short gameObjectId)
    {
        for (var i = 0; i < count; i++)
        {
            AddGameObject(gameObjectId);
        }
    }


}
