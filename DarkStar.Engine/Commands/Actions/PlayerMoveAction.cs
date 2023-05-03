using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Commands;
using DarkStar.Api.Engine.Types.Commands;
using DarkStar.Network.Protocol.Messages.Common;

namespace DarkStar.Engine.Commands.Actions;

public struct PlayerMoveAction : ICommandAction
{
    public double Delay { get; set; }
    public CommandActionType Type { get; } = CommandActionType.PlayerMove;
    public MoveDirectionType Direction { get; set; }

    public string SessionId { get; set; }

    public Guid PlayerId { get; set; }

    public PlayerMoveAction(string sessionId, Guid playerId, MoveDirectionType direction, double delay = 1000)
    {
        SessionId = sessionId;
        PlayerId = playerId;
        Delay = delay;
        Direction = direction;
    }
}
