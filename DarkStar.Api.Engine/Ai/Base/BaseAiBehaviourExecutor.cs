using DarkStar.Api.Engine.Interfaces.Ai;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Database.Entities.Npc;
using Microsoft.Extensions.Logging;

namespace DarkStar.Api.Engine.Ai.Base
{
    public class BaseAiBehaviourExecutor : IAiBehaviourExecutor
    {

        /// <summary>
        /// Default interval is 1 second
        /// </summary>
        public double Interval { get; set; } = 1000;
        private double _currentInterval = 1000;

        protected NpcGameObject NpcGameObject { get; private set; } = null!;
        protected NpcEntity NpcEntity { get; private set; } = null!;
        protected string MapId { get; private set; } = null!;


        protected ILogger Logger { get; }
        protected IDarkSunEngine Engine { get; }

        public BaseAiBehaviourExecutor(ILogger<BaseAiBehaviourExecutor> logger, IDarkSunEngine engine)
        {
            Logger = logger;
            Engine = engine;
        }

        public ValueTask ProcessAsync(double delta)
        {
            _currentInterval -= delta;
            if (!(_currentInterval <= 0))
            {
                return ValueTask.CompletedTask;
            }

            _currentInterval = Interval;
            return DoAiAsync();


        }

        public ValueTask InitializeAsync(string mapId, NpcEntity npc, NpcGameObject npcGameObject)
        {
            Logger.LogDebug("Initializing {Name} AI Behaviour", GetType().Name);
            MapId = mapId;
            NpcGameObject = npcGameObject;
            NpcEntity = npc;
            return ValueTask.CompletedTask;
        }

        protected void SetInterval(double interval)
        {
            Interval = interval;
            _currentInterval = interval;
        }

        protected virtual ValueTask DoAiAsync()
        {
            return ValueTask.CompletedTask;
        }

        public virtual void Dispose()
        {

        }
    }
}
