using DarkStar.Api.Engine.Attributes.Network;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.MessageListeners;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Players;
using DarkStar.Network.Protocol.Types;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.MessageListeners;

[NetworkMessageListener(DarkStarMessageType.PlayerLogoutRequest)]
public class PlayerLogoutMessageListener : BaseNetworkMessageListener<PlayerLogoutRequestMessage>
{
    public PlayerLogoutMessageListener(ILogger<PlayerLogoutMessageListener> logger, IDarkSunEngine engine) : base(
        logger,
        engine
    )
    {
    }


    public override async Task<List<IDarkStarNetworkMessage>> OnMessageReceivedAsync(
        string sessionId, DarkStarMessageType messageType, PlayerLogoutRequestMessage message
    )
    {
        var session = Engine.PlayerService.GetSession(sessionId);
        if (session.IsLogged)
        {
            var playersObject = await Engine.WorldService.GetPlayersByMapIdAsync(session.MapId);
            var player = playersObject.First(s => s.ObjectId == session.PlayerId);
            await Engine.WorldService.RemoveEntityAsync(session.MapId, player.ID);

            return SingleMessage(new PlayerLogoutResponseMessage(true));
        }

        return EmptyMessage();
    }
}
