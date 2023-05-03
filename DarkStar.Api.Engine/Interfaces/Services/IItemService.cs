using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Data.Items;
using DarkStar.Api.Engine.Interfaces.Services.Base;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.World.Types.GameObjects;
using DarkStar.Network.Protocol.Messages.Common;

namespace DarkStar.Api.Engine.Interfaces.Services;

public interface IItemService : IDarkSunEngineService
{
    Task ExecuteGameObjectActionAsync(
        WorldGameObject gameObject, string mapId, Guid? sessionId, Guid? playerId, bool isNpc, uint? npcId, Guid? npcObjectId
    );

    void AddScriptableGameObject(GameObjectType gameObjectType, Action<GameObjectContext> callBack);
    void AddScriptableScheduledGameObject(GameObjectType gameObjectType, int delay, Action<GameObjectContext> callBack);
}
