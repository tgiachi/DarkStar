using DarkStar.Api.Engine.Types.Commands;
using DarkStar.Network.Protocol.Messages.Common;
using Redbus.Events;

namespace DarkStar.Api.Engine.Events.Commands;

public class GameObjectCommandEvent : EventBase
{
    public double Delay { get; set; } = 500;
    public string MapId { get; set; } = null!;
    public CommandActionType Type { get; } = CommandActionType.GameObjectAction;
    public Guid? SessionId { get; set; }
    public Guid? PlayerId { get; set; }
    public bool IsNpc { get; set; }
    public uint? NpcId { get; set; }
    public Guid? NpcObjectId { get; set; }
    public PointPosition Position { get; set; }
}
