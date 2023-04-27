using DarkStar.Api.Engine.Ai.Base;
using DarkStar.Api.Engine.Attributes.Ai;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Utils;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Messages.World;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Ai.Animals;


[AiBehaviour("Animal", "Cat")]
public class CatAi : BaseAiBehaviourExecutor
{
    public CatAi(ILogger<CatAi> logger, IDarkSunEngine engine) : base(logger, engine)
    {
    }

    protected override async ValueTask DoAiAsync()
    {
        MoveToDirection(MoveDirectionType.East.RandomEnumValue());
        if (RandomUtils.RandomBool())
        {
            Logger.LogInformation("Meow!");
            await SendWorldMessageAsync("Meow!", WorldMessageType.Yell);
        }
    }
}
