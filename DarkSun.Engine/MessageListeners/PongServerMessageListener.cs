using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Attributes;
using DarkSun.Api.Engine.Attributes.Network;
using DarkSun.Api.Engine.Interfaces.Core;
using DarkSun.Api.Engine.MessageListeners;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Live;
using DarkSun.Network.Protocol.Types;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.MessageListeners
{
    [NetworkMessageListener(DarkSunMessageType.Pong)]
    public class PongMessageListener : BaseNetworkMessageListener<PongMessageResponse>
    {
        public PongMessageListener(ILogger<BaseNetworkMessageListener<PongMessageResponse>> logger,
            IDarkSunEngine engine) :
            base(logger, engine)
        {
        }

        public override Task<List<IDarkSunNetworkMessage>> OnMessageReceivedAsync(Guid sessionId,
            DarkSunMessageType messageType,
            PongMessageResponse message)
        {
            Logger.LogInformation("Received PONG from session {Session}", sessionId);
            Engine.PlayerService.GetSession(sessionId).LastPingDateTime = DateTime.Now;
            return Task.FromResult(EmptyMessage());
        }
    }
}
