using DarkStar.Api.Engine.Interfaces.Objects;
using Microsoft.Extensions.Logging;

namespace DarkStar.Api.Engine.Items.WorldObjects.Base;

public class BaseScheduledWorldObject : BaseWorldObjectAction, IScheduledGameObjectAction
{
    private double _currentInterval = 1000;
    public double Interval { get; set; } = 1000;
    public BaseScheduledWorldObject(ILogger<BaseWorldObjectAction> logger) : base(logger)
    {
    }

    public ValueTask UpdateAsync(double deltaTime)
    {
        _currentInterval -= deltaTime;
        if (!(_currentInterval <= 0))
        {
            return ValueTask.CompletedTask;
        }

        _currentInterval = Interval;
        return OnActivatedAsync(MapId, GameObject, Guid.Empty, false);
    }

    protected void SetInterval(double interval)
    {
        Interval = interval;
        _currentInterval = interval;
    }
}
