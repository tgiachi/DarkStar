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
using DarkStar.Network.Protocol.Messages.TileSet;
using DarkStar.Network.Protocol.Types;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.MessageListeners
{

    [NetworkMessageListener(DarkStarMessageType.TileSetDownloadRequest)]
    public class TileSetDownloadMessageListener : BaseNetworkMessageListener<TileSetDownloadRequestMessage>
    {
        public TileSetDownloadMessageListener(ILogger<BaseNetworkMessageListener<TileSetDownloadRequestMessage>> logger, IDarkSunEngine engine) : base(logger, engine)
        {
        }

        public override async Task<List<IDarkStarNetworkMessage>> OnMessageReceivedAsync(Guid sessionId, DarkStarMessageType messageType, TileSetDownloadRequestMessage message)
        {
            if (!Engine.PlayerService.GetSession(sessionId).IsLogged)
            {
                return EmptyMessage();
            }

            var result = await TileSetHelper.GetTileSetAsync(message.TileName, Engine);
            return SingleMessage(new TileSetDownloadResponseMessage(message.TileName, result));

        }
    }
}
