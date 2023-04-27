using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Network.Protocol.Messages.Common;

namespace DarkStar.Api.Engine.Data.Blueprint;
public class BlueprintGenerationMapContext
{
    private readonly SemaphoreSlim _listLock = new(1);
    private readonly IBlueprintService _blueprintService;
    private readonly ITypeService _typeService;
    private readonly INamesService _namesService;
    private readonly IWorldService _worldService;
    private readonly string _mapId;

    private readonly List<WorldGameObject> _gameObjects = new();
    private readonly List<NpcGameObject> _npcs = new();

    public List<WorldGameObject> GameObjects => _gameObjects;
    public List<NpcGameObject> Npcs => _npcs;
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

    public async void AddGameObject(short gameObjectId)
    {
        await BlueprintService.GenerateWorldGameObjectAsync(
                _typeService.GetGameObjectType(gameObjectId),
                WorldService.GetRandomWalkablePosition(_mapId)
            )
            .ContinueWith(
                task =>
                {
                    _listLock.Wait();
                    _gameObjects.Add(task.Result);
                    _listLock.Release();

                }, TaskScheduler.Current);
    }

    public void AddGameObjects(int count, short gameObjectId)
    {
        for (var i = 0; i < count; i++)
        {
            AddGameObject(gameObjectId);
        }
    }

    public async void AddNpc(short npcType, short subType, int level = 1)
    {
        await BlueprintService.GenerateNpcGameObjectAsync(WorldService.GetRandomWalkablePosition(_mapId), _typeService.GetNpcType(npcType), _typeService.GetNpcSubType(subType), level)
            .ContinueWith(
                task =>
                {
                    _listLock.Wait();
                    _npcs.Add(task.Result);
                    _listLock.Release();
                }, TaskScheduler.Current);
    }

    public void AddNpcs(int count, short npcType, short subType, int level = 1)
    {
        for (var i = 0; i < count; i++)
        {
            AddNpc(npcType, subType, level);
        }
    }


}
