using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Commands;
using DarkStar.Api.Engine.Types.Commands;
using DarkStar.Network.Protocol.Messages.Common;

namespace DarkStar.Engine.Commands.Actions;
public class GameObjectAction : ICommandAction
{
    public double Delay { get; set; } = 500;
    public CommandActionType Type { get; } = CommandActionType.GameObjectAction;
    public Guid? SessionId { get; set; }
    public Guid? PlayerId { get; set; }
    public bool IsNpc { get; set; }
    public uint? NpcId { get; set; }
    public Guid? NpcObjectId { get; set; }

    public PointPosition Position { get; set; }

}
