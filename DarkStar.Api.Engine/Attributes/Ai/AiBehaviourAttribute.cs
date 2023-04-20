using DarkStar.Api.World.Types.Npc;

namespace DarkStar.Api.Engine.Attributes.Ai;


[AttributeUsage(AttributeTargets.Class)]
public class AiBehaviourAttribute : Attribute
{
    public NpcType Type { get; }

    public NpcSubType SubType { get; }

    public AiBehaviourAttribute(NpcType type, NpcSubType subType)
    {
        Type = type;
        SubType = subType;
    }
}
