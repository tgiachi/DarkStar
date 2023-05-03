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

[NetworkMessageListener(DarkStarMessageType.PlayerRacesRequest)]
public class RacesRequestMessageListener : BaseNetworkMessageListener<PlayerRacesRequestMessage>
{
    public RacesRequestMessageListener(
        ILogger<BaseNetworkMessageListener<PlayerRacesRequestMessage>> logger,
        IDarkSunEngine engine
    ) : base(logger, engine)
    {
    }

    public override async Task<List<IDarkStarNetworkMessage>> OnMessageReceivedAsync(
        string sessionId,
        DarkStarMessageType messageType, PlayerRacesRequestMessage message
    ) =>
        Engine.PlayerService.GetSession(sessionId).IsLogged
            ? SingleMessage(await PlayerDataHelper.BuildPlayerRacesAsync(Engine))
            : EmptyMessage();
}
