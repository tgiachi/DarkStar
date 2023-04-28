
using DarkStar.Api.Engine.Attributes.Network;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.MessageListeners;
using DarkStar.Engine.Commands.Actions;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Players;
using DarkStar.Network.Protocol.Types;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.MessageListeners;

[NetworkMessageListener(DarkStarMessageType.PlayerMoveRequest)]
public class PlayerMoveMessageListener : BaseNetworkMessageListener<PlayerMoveRequestMessage>
{
    public PlayerMoveMessageListener(ILogger<BaseNetworkMessageListener<PlayerMoveRequestMessage>> logger, IDarkSunEngine engine) : base(logger, engine)
    {
    }

    public override Task<List<IDarkStarNetworkMessage>> OnMessageReceivedAsync(string sessionId, DarkStarMessageType messageType, PlayerMoveRequestMessage message)
    {
        if (Engine.PlayerService.GetSession(sessionId).IsLogged)
        {
            Engine.CommandService.EnqueuePlayerAction(new PlayerMoveAction(sessionId, Engine.PlayerService.GetSession(sessionId).PlayerId, message.Direction));
        }

        return Task.FromResult(EmptyMessage());
    }
}
