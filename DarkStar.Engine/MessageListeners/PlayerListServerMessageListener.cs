using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Attributes.Network;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.MessageListeners;
using DarkStar.Engine.MessageListeners.Helpers;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Players;
using DarkStar.Network.Protocol.Types;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.MessageListeners;

[NetworkMessageListener(DarkStarMessageType.PlayerListResponse)]
public class PlayerListMessageListener : BaseNetworkMessageListener<PlayerListResponseMessage>
{
    public PlayerListMessageListener(
        ILogger<BaseNetworkMessageListener<PlayerListResponseMessage>> logger,
        IDarkSunEngine engine
    ) : base(logger, engine)
    {
    }

    public override async Task<List<IDarkStarNetworkMessage>> OnMessageReceivedAsync(
        string sessionId,
        DarkStarMessageType messageType, PlayerListResponseMessage message
    )
    {
        var playerSession = Engine.PlayerService.GetSession(sessionId);
        if (playerSession.IsLogged)
        {
            return SingleMessage(
                await PlayerDataHelper.BuildPlayerListForPlayerAsync(Engine, playerSession.PlayerId)
            );
        }

        return EmptyMessage();
    }
}
