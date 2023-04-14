using DarkSun.Api.Engine.Map.Entities;
using DarkSun.Database.Entities.Npc;

namespace DarkSun.Api.Engine.Interfaces.Ai;

public interface IAiBehaviourExecutor
{
    Task ProcessAsync(NpcEntity npcEntity, NpcGameObject gameObject, double delta);
}
