using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Database.Entities.Npc;

namespace DarkStar.Api.Engine.Interfaces.Ai;

public interface IAiBehaviourExecutor : IDisposable
{
    double Interval { get; }
    ValueTask ProcessAsync(double delta);
    ValueTask InitializeAsync(string mapId, NpcEntity npc, NpcGameObject npcGameObject);
}
