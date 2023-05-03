using DarkStar.Api.World.Types.GameObjects;

namespace DarkStar.Api.Engine.Attributes.Objects;

[AttributeUsage(AttributeTargets.Class)]
public class GameObjectActionAttribute : Attribute
{
    public string GameObjectType { get; set; }

    public GameObjectActionAttribute(string type) => GameObjectType = type;
}
