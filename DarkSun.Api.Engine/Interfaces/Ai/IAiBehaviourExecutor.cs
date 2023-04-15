using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Database.Entities.Npc;

namespace DarkStar.Api.Engine.Interfaces.Ai;

public interface IAiBehaviourExecutor
{
    Task ProcessAsync(NpcEntity npcEntity, NpcGameObject gameObject, double delta);
}
