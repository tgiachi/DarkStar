using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Attributes.Network;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.MessageListeners;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Live;
using DarkStar.Network.Protocol.Types;

using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.MessageListeners
{
    [NetworkMessageListener(DarkStarMessageType.Pong)]
    public class PongMessageListener : BaseNetworkMessageListener<PongMessageResponse>
    {
        public PongMessageListener(ILogger<BaseNetworkMessageListener<PongMessageResponse>> logger,
            IDarkSunEngine engine) :
            base(logger, engine)
        {
        }

        public override Task<List<IDarkStarNetworkMessage>> OnMessageReceivedAsync(Guid sessionId,
            DarkStarMessageType messageType,
            PongMessageResponse message)
        {
            Logger.LogInformation("Received PONG from session {Session}", sessionId);
            Engine.PlayerService.GetSession(sessionId).LastPingDateTime = DateTime.Now;
            return Task.FromResult(EmptyMessage());
        }
    }
}
