using DarkStar.Api.Engine.Attributes.Network;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.MessageListeners;
using DarkStar.Database.Entities.TileSets;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.TileSet;
using DarkStar.Network.Protocol.Types;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.MessageListeners;

[NetworkMessageListener(DarkStarMessageType.TileSetListRequest)]
public class TileSetListMessageListener : BaseNetworkMessageListener<TileSetListRequestMessage>
{
    public TileSetListMessageListener(
        ILogger<BaseNetworkMessageListener<TileSetListRequestMessage>> logger, IDarkSunEngine engine
    ) : base(logger, engine)
    {
    }

    public override async Task<List<IDarkStarNetworkMessage>> OnMessageReceivedAsync(
        string sessionId, DarkStarMessageType messageType, TileSetListRequestMessage message
    )
    {
        var tileSets = await Engine.DatabaseService.FindAllAsync<TileSetEntity>();

        return SingleMessage(
            new TileSetListResponseMessage()
            {
                TileSets = tileSets.Select(
                        x => new TileSetEntryMessage()
                        {
                            Id = x.Id.ToString(),
                            Name = x.Name,
                            FileSize = x.FileSize,
                            TileHeight = x.TileHeight,
                            TileWidth = x.TileWidth
                        }
                    )
                    .ToList()
            }
        );
    }
}
