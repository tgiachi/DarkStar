using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Attributes;
using DarkSun.Api.Engine.Interfaces.Core;
using DarkSun.Api.Engine.MessageListeners;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Messages.Players;
using DarkSun.Network.Protocol.Types;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.MessageListeners
{
    [NetworkMessageListener(DarkSunMessageType.PlayerSelectRequest)]
    public class PlayerSelectMessageListener : BaseNetworkMessageListener<PlayerSelectRequestMessage>
    {
        public PlayerSelectMessageListener(ILogger<BaseNetworkMessageListener<PlayerSelectRequestMessage>> logger, IDarkSunEngine engine) : base(logger, engine)
        {
        }

        public override Task<List<IDarkSunNetworkMessage>> OnMessageReceivedAsync(Guid sessionId, DarkSunMessageType messageType, PlayerSelectRequestMessage message)
        {
            return base.OnMessageReceivedAsync(sessionId, messageType, message);
        }
    }
}
