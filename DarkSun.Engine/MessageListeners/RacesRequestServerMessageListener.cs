using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Attributes;
using DarkSun.Api.Engine.Attributes.Network;
using DarkSun.Api.Engine.Interfaces.Core;
using DarkSun.Api.Engine.MessageListeners;
using DarkSun.Engine.MessageListeners.Helpers;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Messages.Players;
using DarkSun.Network.Protocol.Types;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.MessageListeners
{
    [NetworkMessageListener(DarkSunMessageType.PlayerRacesRequest)]
    public class RacesRequestMessageListener : BaseNetworkMessageListener<PlayerRacesRequestMessage>
    {
        public RacesRequestMessageListener(ILogger<BaseNetworkMessageListener<PlayerRacesRequestMessage>> logger,
            IDarkSunEngine engine) : base(logger, engine)
        {
        }

        public override async Task<List<IDarkSunNetworkMessage>> OnMessageReceivedAsync(Guid sessionId,
            DarkSunMessageType messageType, PlayerRacesRequestMessage message)
        {
            return Engine.PlayerService.GetSession(sessionId).IsLogged
                ? SingleMessage(await PlayerDataHelper.BuildPlayerRacesAsync(Engine))
                : EmptyMessage();
        }
    }
}
