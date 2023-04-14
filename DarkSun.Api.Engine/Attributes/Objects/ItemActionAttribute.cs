using DarkSun.Api.World.Types.Items;

namespace DarkSun.Api.Engine.Attributes.Objects;

[AttributeUsage(AttributeTargets.Class)]
public class ItemActionAttribute : Attribute
{
    public ItemType Type { get; set; }

    public ItemActionAttribute(ItemType type)
    {
        Type = type;
    }
}
