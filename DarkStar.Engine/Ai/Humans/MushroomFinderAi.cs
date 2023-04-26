using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Ai.Base;
using DarkStar.Api.Engine.Attributes.Ai;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.Engine.Utils;
using DarkStar.Api.Utils;
using DarkStar.Api.World.Types.GameObjects;
using DarkStar.Api.World.Types.Map;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Engine.Commands.Actions;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Messages.World;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Ai.Humans;

[AiBehaviour("Human", "Mushroom_Finder")]
public class MushroomFinderAi : BaseAiBehaviourExecutor
{
    private int _currentStep = 0;
    private List<PointPosition> _path = new();

    public MushroomFinderAi(ILogger<BaseAiBehaviourExecutor> logger, IDarkSunEngine engine) : base(logger, engine)
    {
        SetInterval(1000);
    }

    protected override async ValueTask DoAiAsync()
    {
        if (_path.Count > 0)
        {
            if (_currentStep >= _path.Count)
            {
                Logger.LogInformation("MMMhh, yummy mushroom!");
                await SendWorldMessageAsync("MMMhh, yummy mushroom!", WorldMessageType.Yell);

                Engine.CommandService.EnqueueNpcAction(new GameObjectAction()
                {
                    MapId = MapId,
                    IsNpc = true,
                    NpcId = NpcGameObject.ID,
                    NpcObjectId = NpcEntity.Id,
                    Position = NpcGameObject.PointPosition(),
                });

                _path.Clear();
                _currentStep = 0;

                return;
            }

            var nextStep = _path[_currentStep];
            Logger.LogInformation("I'm {Name} and i'm moving to {NewPos} - Step remaining: {StepRemain}", NpcEntity.Name, nextStep, _path.Count - _currentStep);
            if (MoveToPosition(nextStep))
            {
                _currentStep++;
            }
            return;
        }

        WorldGameObject mushrooms = null; //(await GetEntitiesInRangeAsync<WorldGameObject>(MapLayer.Objects)).FirstOrDefault(s => s.Type == GameObjectType.Prop_Mushroom);

        if (mushrooms == null)
        {
            Logger.LogInformation("Sigh, no mushroom found :( !");
            MoveRandomDirection();
        }
        else
        {
            Logger.LogInformation("My name is {Name} and I found mushroom!", NpcEntity.Name);
            _path = GetPathToPosition(mushrooms.PointPosition());
            _currentStep = 0;
        }
    }
}
