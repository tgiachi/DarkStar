namespace DarkStar.Client.Models.Events;

public class ProgressUpdateEvent
{
    public int Progress { get; set; }

    public string Message { get; set; }

    public int MaxProgress { get; set; }

    public ProgressUpdateEvent(string message, int progress = 0,  int maxProgress = 100)
    {
        Progress = progress;
        Message = message;
        MaxProgress = maxProgress;
    }
    public ProgressUpdateEvent() {}
}
