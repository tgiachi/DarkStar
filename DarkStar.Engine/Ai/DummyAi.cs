using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Ai.Base;
using DarkStar.Api.Engine.Attributes.Ai;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.World.Types.Npc;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Ai;


[AiBehaviour(NpcType.Animal, NpcSubType.Cat)]
public class DummyAi : BaseAiBehaviourExecutor
{
    public DummyAi(ILogger<DummyAi> logger, IDarkSunEngine engine) : base(logger, engine)
    {
    }
}
