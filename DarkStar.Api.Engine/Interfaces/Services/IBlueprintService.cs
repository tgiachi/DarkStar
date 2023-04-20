using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Services.Base;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Database.Entities.Npc;
using DarkStar.Network.Protocol.Messages.Common;

namespace DarkStar.Api.Engine.Interfaces.Services;

public interface IBlueprintService : IDarkSunEngineService
{

    Task<NpcEntity> GenerateNpcEntityAsync(NpcType npcType, NpcSubType subType, int level = 1);

    Task<NpcGameObject> GenerateNpcGameObjectAsync(PointPosition position, NpcType npcType, NpcSubType subType, int level = 1);

}
