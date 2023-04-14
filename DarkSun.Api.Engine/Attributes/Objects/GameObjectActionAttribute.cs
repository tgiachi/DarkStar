using DarkStar.Api.World.Types.GameObjects;

namespace DarkStar.Api.Engine.Attributes.Objects
{

    [AttributeUsage(AttributeTargets.Class)]
    public class GameObjectActionAttribute : Attribute
    {
        public GameObjectType Type { get; set; }

        public GameObjectActionAttribute(GameObjectType type)
        {
            Type = type;
        }
    }
}
