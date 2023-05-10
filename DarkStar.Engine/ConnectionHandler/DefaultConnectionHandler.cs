using System.Reflection;
using DarkStar.Api.Engine.Attributes.Network;
using DarkStar.Api.Engine.ConnectionHandlers;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Utils;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Server;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.ConnectionHandler;

[NetworkConnectionHandler]
public class DefaultConnectionHandler : BaseNetworkConnectionHandler
{
    private static readonly Version s_version = Assembly.GetExecutingAssembly().GetName().Version;

    public DefaultConnectionHandler(ILogger<DefaultConnectionHandler> logger, IDarkSunEngine engine) : base(
        logger,
        engine
    )
    {
    }

    public override Task<List<IDarkStarNetworkMessage>> ClientConnectedMessagesAsync(string sessionId)
    {
        Logger.LogInformation("New connection: {Id}", sessionId);
        Engine.PlayerService.AddSession(sessionId);
        return Task.FromResult(
            new List<IDarkStarNetworkMessage>
            {
                new ServerNameResponseMessage(Engine.ServerName),
                new ServerVersionResponseMessage(s_version.Minor, s_version.Minor, s_version.Build)
            }
        );
    }

    public override Task ClientDisconnectedAsync(string sessionId)
    {
        Engine.PlayerService.RemoveSession(sessionId);
        return Task.CompletedTask;
    }
}
