using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Attributes.Network;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.MessageListeners;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Players;
using DarkStar.Network.Protocol.Types;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.MessageListeners;

[NetworkMessageListener(DarkStarMessageType.PlayerSelectRequest)]
public class PlayerSelectMessageListener : BaseNetworkMessageListener<PlayerSelectRequestMessage>
{
    public PlayerSelectMessageListener(
        ILogger<BaseNetworkMessageListener<PlayerSelectRequestMessage>> logger,
        IDarkSunEngine engine
    ) : base(logger, engine)
    {
    }

    public override Task<List<IDarkStarNetworkMessage>> OnMessageReceivedAsync(
        string sessionId,
        DarkStarMessageType messageType, PlayerSelectRequestMessage message
    ) =>
        base.OnMessageReceivedAsync(sessionId, messageType, message);
}
