namespace DarkStar.Api.Engine.Interfaces.Objects;

public interface IScheduledGameObjectAction : IGameObjectAction
{
    double Interval { get; set; }
    ValueTask UpdateAsync(double deltaTime);
}
