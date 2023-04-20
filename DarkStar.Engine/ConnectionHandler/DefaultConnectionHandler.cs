using DarkStar.Api.Engine.Attributes.Network;
using DarkStar.Api.Engine.ConnectionHandlers;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Server;

using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.ConnectionHandler;

[NetworkConnectionHandler]
public class DefaultConnectionHandler : BaseNetworkConnectionHandler
{
    public DefaultConnectionHandler(ILogger<BaseNetworkConnectionHandler> logger, IDarkSunEngine engine) : base(
        logger,
        engine)
    {
    }

    public override Task<List<IDarkStarNetworkMessage>> ClientConnectedMessagesAsync(Guid sessionId)
    {
        Logger.LogInformation("New connection: {Id}", sessionId);
        Engine.PlayerService.AddSession(sessionId);
        return Task.FromResult(new List<IDarkStarNetworkMessage>
        {
            new ServerNameResponseMessage(Engine.ServerName), new ServerVersionResponseMessage(0, 0, 1)
        });
    }

    public override Task ClientDisconnectedAsync(Guid sessionId)
    {
        Engine.PlayerService.RemoveSession(sessionId);
        return Task.CompletedTask;
    }
}
