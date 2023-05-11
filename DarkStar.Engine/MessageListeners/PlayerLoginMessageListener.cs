using DarkStar.Api.Engine.Attributes.Network;
using DarkStar.Api.Engine.Events.Players;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.MessageListeners;
using DarkStar.Database.Entities.Player;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Messages.Players;
using DarkStar.Network.Protocol.Types;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.MessageListeners;

[NetworkMessageListener(DarkStarMessageType.PlayerLoginRequest)]
public class PlayerLoginMessageListener : BaseNetworkMessageListener<PlayerLoginRequestMessage>
{
    public PlayerLoginMessageListener(ILogger<PlayerLoginMessageListener> logger, IDarkSunEngine engine) : base(
        logger,
        engine
    )
    {
    }

    public override async Task<List<IDarkStarNetworkMessage>> OnMessageReceivedAsync(
        string sessionId, DarkStarMessageType messageType, PlayerLoginRequestMessage message
    )
    {
        if (Engine.PlayerService.GetSession(sessionId).IsLogged)
        {
            var player = await Engine.DatabaseService.QueryAsSingleAsync<PlayerEntity>(
                entity => entity.AccountId == Engine.PlayerService.GetSession(sessionId).AccountId
                          && (entity.Id == message.PlayerId || entity.Name == message.PlayerName)
            );

            Engine.PlayerService.GetSession(sessionId).PlayerId = player.Id;
            Engine.PlayerService.GetSession(sessionId).MapId = player.MapId;
            Engine.PlayerService.GetSession(sessionId).Position = new PointPosition(player.X, player.Y);

            Logger.LogInformation(
                "Player {Name} logged in @ {Position}",
                player.Name,
                new PointPosition(player.X, player.Y)
            );

            Engine.WorldService.AddPlayerOnMap(
                player.MapId,
                player.Id,
                sessionId,
                new PointPosition(player.X, player.Y),
                player.TileId
            );

            // Send player Data see: {NetworkEventDispatcherService}
            Engine.EventBus.PublishAsync(
                new PlayerLoggedEvent(sessionId, player.Id, player.MapId, PointPosition.New(player.X, player.Y))
            );
        }

        return EmptyMessage();
    }
}
