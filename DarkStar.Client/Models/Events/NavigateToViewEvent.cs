using System;

namespace DarkStar.Client.Models.Events;

public class NavigateToViewEvent
{
    public Type ViewType { get; set; }

    public NavigateToViewEvent(Type viewType)
    {
        ViewType = viewType;
    }

    public NavigateToViewEvent()
    {

    }
}
