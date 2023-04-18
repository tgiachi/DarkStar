﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Attributes.Commands;
using DarkStar.Api.Engine.Commands.Base;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Types.Commands;
using DarkStar.Engine.Commands.Actions;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Commands
{
    [CommandAction(CommandActionType.PlayerMove)]
    public class PlayerCommandActionExecutor : BaseCommandActionExecutor<PlayerMoveAction>
    {
        public PlayerCommandActionExecutor(ILogger<BaseCommandActionExecutor<PlayerMoveAction>> logger, IDarkSunEngine engine) : base(logger, engine)
        {

        }

        public override Task ProcessAsync(PlayerMoveAction action)
        {
            return base.ProcessAsync(action);
        }
    }
}