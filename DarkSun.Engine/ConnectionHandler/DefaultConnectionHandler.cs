using DarkSun.Api.Engine.Attributes;
using DarkSun.Api.Engine.ConnectionHandlers;
using DarkSun.Api.Engine.Interfaces.Core;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Server;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.ConnectionHandler;

[NetworkConnectionHandler]
public class DefaultConnectionHandler : BaseNetworkConnectionHandler
{
    public DefaultConnectionHandler(ILogger<BaseNetworkConnectionHandler> logger, IDarkSunEngine engine) : base(logger,
        engine)
    {
    }

    public override Task<List<IDarkSunNetworkMessage>> ClientConnectedMessagesAsync(Guid sessionId)
    {
        Logger.LogInformation("New connection: {Id}", sessionId);
        Engine.PlayerSessionService.AddPlayerSession(sessionId);
        return Task.FromResult(new List<IDarkSunNetworkMessage>() { new ServerVersionResponseMessage(0, 1, 0) });
    }

    public override Task ClientDisconnectedAsync(Guid sessionId)
    {
        Engine.PlayerSessionService.RemovePlayerSession(sessionId);
        return Task.CompletedTask;
    }
}
