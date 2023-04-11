using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Interfaces.Core;
using DarkSun.Api.Engine.Interfaces.Listener;
using DarkSun.Network.Protocol.Interfaces.Messages;
using Microsoft.Extensions.Logging;

namespace DarkSun.Api.Engine.ConnectionHandlers;

public class BaseNetworkConnectionHandler : INetworkConnectionHandler
{
    protected ILogger Logger { get; }
    protected IDarkSunEngine Engine { get; }

    public BaseNetworkConnectionHandler(ILogger<BaseNetworkConnectionHandler> logger, IDarkSunEngine engine)
    {
        Logger = logger;
        Engine = engine;
    }

    public virtual Task<List<IDarkSunNetworkMessage>> ClientConnectedMessagesAsync(Guid sessionId)
    {
        throw new NotImplementedException();
    }

    public virtual Task ClientDisconnectedAsync(Guid sessionId)
    {
        throw new NotImplementedException();
    }
}
