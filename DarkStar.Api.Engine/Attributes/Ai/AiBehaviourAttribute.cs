using DarkStar.Api.World.Types.Npc;

namespace DarkStar.Api.Engine.Attributes.Ai;

[AttributeUsage(AttributeTargets.Class)]
public class AiBehaviourAttribute : Attribute
{
    public string NpcType { get; }

    public string NpcSubType { get; }

    public AiBehaviourAttribute(string npcType, string npcSubType)
    {
        NpcType = npcType;
        NpcSubType = npcSubType;
    }
}
