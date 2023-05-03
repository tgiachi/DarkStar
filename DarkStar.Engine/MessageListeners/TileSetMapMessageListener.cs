using DarkStar.Api.Engine.Attributes.Network;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.MessageListeners;
using DarkStar.Engine.MessageListeners.Helpers;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.TileSet;
using DarkStar.Network.Protocol.Types;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.MessageListeners;

[NetworkMessageListener(DarkStarMessageType.TileSetMapRequest)]
public class TileSetMapMessageListener : BaseNetworkMessageListener<TileSetMapRequestMessage>
{
    public TileSetMapMessageListener(
        ILogger<BaseNetworkMessageListener<TileSetMapRequestMessage>> logger, IDarkSunEngine engine
    ) : base(logger, engine)
    {
    }

    public override async Task<List<IDarkStarNetworkMessage>> OnMessageReceivedAsync(
        string sessionId,
        DarkStarMessageType messageType, TileSetMapRequestMessage message
    )
    {
        if (Engine.PlayerService.GetSession(sessionId).IsLogged)
        {
            return SingleMessage(
                new TileSetMapResponseMessage(
                    message.TileSetName,
                    await TileSetHelper.GetTileSetMapAsync(message.TileSetName, Engine)
                )
            );
        }

        return EmptyMessage();
    }
}
