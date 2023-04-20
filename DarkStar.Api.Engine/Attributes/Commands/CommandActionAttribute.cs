using DarkStar.Api.Engine.Types.Commands;

namespace DarkStar.Api.Engine.Attributes.Commands;

[AttributeUsage(AttributeTargets.Class)]
public class CommandActionAttribute : Attribute
{
    public CommandActionType Type { get; set; }

    public CommandActionAttribute(CommandActionType type)
    {
        Type = type;
    }

}
