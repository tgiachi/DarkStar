using DarkStar.Api.World.Types.Items;

namespace DarkStar.Api.Engine.Attributes.Objects;

[AttributeUsage(AttributeTargets.Class)]
public class ItemActionAttribute : Attribute
{
    public ushort Type { get; set; }

    public ItemActionAttribute(ushort itemType) => Type = itemType;
}
