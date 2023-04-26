using DarkStar.Api.World.Types.Npc;

namespace DarkStar.Api.Engine.Attributes.Ai;


[AttributeUsage(AttributeTargets.Class)]
public class AiBehaviourAttribute : Attribute
{
    public ushort NpcType { get; }

    public ushort NpcSubType { get; }

    public AiBehaviourAttribute(ushort npcType, ushort npcSubType)
    {
        NpcType = npcType;
        NpcSubType = npcSubType;
    }
}
