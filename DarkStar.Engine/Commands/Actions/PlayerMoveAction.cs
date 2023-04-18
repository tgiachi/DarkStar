using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DarkStar.Api.Engine.Interfaces.Commands;
using DarkStar.Api.Engine.Types.Commands;
using DarkStar.Network.Protocol.Messages.Common;

namespace DarkStar.Engine.Commands.Actions
{
    public class PlayerMoveAction : ICommandAction
    {
        public double Delay { get; set; }
        public CommandActionType Type { get; } = CommandActionType.PlayerMove;

        public PlayerMoveDirectionType Direction { get; set; }

        public PlayerMoveAction(PlayerMoveDirectionType direction, double delay = 1000)
        {
            Delay = delay;
            Direction = direction;
        }
    }
}
