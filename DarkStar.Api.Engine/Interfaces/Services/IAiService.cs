using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Data.Ai;
using DarkStar.Api.Engine.Interfaces.Services.Base;
using DarkStar.Api.World.Types.Npc;

namespace DarkStar.Api.Engine.Interfaces.Services;

public interface IAiService : IDarkSunEngineService
{
    void AddAiScriptByType(NpcType npcType, NpcSubType npcSubType, Action<AiContext> context);
    void AddAiScriptByName(string name, Action<AiContext> context);

}
