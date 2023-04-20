using DarkStar.Api.Engine.Ai.Base;
using DarkStar.Api.Engine.Attributes.Ai;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Utils;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Network.Protocol.Messages.Common;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Ai.Animals;


[AiBehaviour(NpcType.Animal, NpcSubType.Cat)]
public class CatAi : BaseAiBehaviourExecutor
{
    public CatAi(ILogger<CatAi> logger, IDarkSunEngine engine) : base(logger, engine)
    {
    }

    protected override ValueTask DoAiAsync()
    {
        MoveToDirection(MoveDirectionType.East.RandomEnumValue());

        return ValueTask.CompletedTask;
    }
}
