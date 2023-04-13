using DarkSun.Api.Engine.Attributes;
using DarkSun.Api.Engine.Attributes.Network;
using DarkSun.Api.Engine.ConnectionHandlers;
using DarkSun.Api.Engine.Interfaces.Core;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Live;
using DarkSun.Network.Protocol.Messages.Server;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.ConnectionHandler
{
    [NetworkConnectionHandler]
    public class DefaultConnectionHandler : BaseNetworkConnectionHandler
    {
        public DefaultConnectionHandler(ILogger<BaseNetworkConnectionHandler> logger, IDarkSunEngine engine) : base(
            logger,
            engine)
        {
        }

        public override Task<List<IDarkSunNetworkMessage>> ClientConnectedMessagesAsync(Guid sessionId)
        {
            Logger.LogInformation("New connection: {Id}", sessionId);
            Engine.PlayerService.AddSession(sessionId);
            return Task.FromResult(new List<IDarkSunNetworkMessage>
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
}
