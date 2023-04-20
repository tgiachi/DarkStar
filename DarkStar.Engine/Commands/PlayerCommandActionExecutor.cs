using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Attributes.Commands;
using DarkStar.Api.Engine.Commands.Base;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Types.Commands;
using DarkStar.Api.Engine.Utils;
using DarkStar.Engine.Commands.Actions;
using DarkStar.Network.Protocol.Messages.Players;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Commands;

[CommandAction(CommandActionType.PlayerMove)]
public class PlayerCommandActionExecutor : BaseCommandActionExecutor<PlayerMoveAction>
{
    public PlayerCommandActionExecutor(ILogger<BaseCommandActionExecutor<PlayerMoveAction>> logger, IDarkSunEngine engine) : base(logger, engine)
    {

    }

    public override async Task ProcessAsync(PlayerMoveAction action)
    {
        Logger.LogDebug("Player {PlayerId} request move to {Direction}", action.PlayerId, action.Direction);
        var player = Engine.PlayerService.GetSession(action.SessionId);
        var newPosition = player.Position.AddMovement(action.Direction);


        Logger.LogDebug("Player {PlayerId} request move to {Direction} Old Position: {OldPosition} - New position: {NewPosition}", action.PlayerId, action.Direction, player.Position, player.Position.AddMovement(action.Direction));
        var canMove = Engine.WorldService.IsLocationWalkable(player.MapId, newPosition);

        if (canMove)
        {
            player.Position = newPosition;
            await Engine.PlayerService.UpdatePlayerPositionAsync(player.PlayerId, player.MapId, newPosition);
            await Engine.NetworkServer.SendMessageAsync(action.SessionId, new PlayerMoveResponseMessage(newPosition));
        }
    }
}
