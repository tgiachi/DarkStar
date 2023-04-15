namespace DarkStar.Api.Engine.Interfaces.Objects
{
    public interface IScheduledGameObjectAction : IGameObjectAction
    {
        Task UpdateAsync(double deltaTime);
    }
}
