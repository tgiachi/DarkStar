using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Data.Config;
using DarkSun.Api.Engine.Attributes;
using DarkSun.Api.Engine.Data.Config;
using DarkSun.Api.Engine.Interfaces.Core;
using DarkSun.Api.Engine.Interfaces.Listener;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Api.Engine.Interfaces.Services.Base;
using DarkSun.Api.Utils;
using DarkSun.Network.Interfaces;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;
using DarkSun.Network.Server.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine;

public class DarkSunEngine : IDarkSunEngine
{
    private readonly ILogger _logger;
    private readonly DirectoriesConfig _directoriesConfig;
    private readonly IServiceProvider _container;
    private readonly EngineConfig _engineConfig;
    private readonly SortedDictionary<int, IDarkSunEngineService> _servicesLoadOrder = new();
    private readonly HashSet<INetworkConnectionHandler> _connectionHandlers = new();

    public IWorldService WorldService { get; }
    public IBlueprintService BlueprintService { get; }
    public ISchedulerService SchedulerService { get; }
    public IScriptEngineService ScriptEngineService { get; }
    public IDarkSunNetworkServer NetworkServer { get; }
    public IPlayerSessionService PlayerSessionService { get; }


    public DarkSunEngine(ILogger<DarkSunEngine> logger,
        DirectoriesConfig directoriesConfig,
        IBlueprintService blueprintService,
        ISchedulerService schedulerService,
        IScriptEngineService scriptEngineService,
        IDarkSunNetworkServer networkServer,
        IWorldService worldService,
        IPlayerSessionService playerSessionService,
        EngineConfig engineConfig,
        IServiceProvider container)
    {
        _logger = logger;
        _directoriesConfig = directoriesConfig;
        WorldService = worldService;
        BlueprintService = blueprintService;
        SchedulerService = schedulerService;
        ScriptEngineService = scriptEngineService;
        NetworkServer = networkServer;
        PlayerSessionService = playerSessionService;
        _engineConfig = engineConfig;
        _container = container;
    }

    private ValueTask PrepareMessageListenersAsync()
    {
        var messageListenersTypes = AssemblyUtils.GetAttribute<NetworkMessageListenerAttribute>();
        foreach (var messageListenerType in messageListenersTypes)
        {
            var attribute = messageListenerType.GetCustomAttribute<NetworkMessageListenerAttribute>()!;
            _logger.LogDebug("Adding message listener {Type} from message type: {MessageType}",
                messageListenerType.Name, attribute.MessageType);
            if (_container.GetService(messageListenerType) is INetworkMessageListener service)
            {
                NetworkServer.RegisterMessageListener(attribute.MessageType, service);
            }
        }

        return ValueTask.CompletedTask;
    }

    private ValueTask PrepareConnectionHandlersAsync()
    {
        NetworkServer.OnClientConnected += NetworkServerOnOnClientConnectedAsync;
        NetworkServer.OnClientDisconnected += NetworkServerOnClientDisconnectedAsync;

        foreach (var connectionHandler in AssemblyUtils.GetAttribute<NetworkConnectionHandlerAttribute>())
        {
            if (_container.GetService(connectionHandler) is INetworkConnectionHandler handler)
            {
                _connectionHandlers.Add(handler);
            }
        }

        return ValueTask.CompletedTask;
    }

    private async Task NetworkServerOnClientDisconnectedAsync(Guid sessionId)
    {
        foreach (var handler in _connectionHandlers)
        {
            await handler.ClientDisconnectedAsync(sessionId);
        }
    }

    private async Task<List<IDarkSunNetworkMessage>> NetworkServerOnOnClientConnectedAsync(Guid sessionId)
    {
        var messages = new List<IDarkSunNetworkMessage>();
        foreach (var handler in _connectionHandlers)
        {
            messages.AddRange(await handler.ClientConnectedMessagesAsync(sessionId));
        }

        return messages;
    }

    public async ValueTask<bool> StartAsync()
    {
        await BuildServicesOrderAsync();
        await PrepareMessageListenersAsync();
        await PrepareConnectionHandlersAsync();

        foreach (var service in _servicesLoadOrder)
        {
            await service.Value.StartAsync(this);
        }

        await NetworkServer.StartAsync();

        return true;
    }

    public async ValueTask<bool> StopAsync()
    {
        await NetworkServer.StopAsync();
        foreach (var service in _servicesLoadOrder.Reverse())
        {
            await service.Value.StopAsync();
        }

        return true;
    }

    private ValueTask BuildServicesOrderAsync()
    {
        _logger.LogDebug("Building services load order");
        var services = AssemblyUtils.GetAttribute<DarkSunEngineServiceAttribute>();
        foreach (var serviceType in services)
        {
            var attr = serviceType.GetCustomAttribute<DarkSunEngineServiceAttribute>()!;
            var interf = AssemblyUtils.GetInterfacesOfType(serviceType)!.First(k => k.Name.EndsWith(serviceType.Name));
            _servicesLoadOrder.Add(attr.LoadOrder, (IDarkSunEngineService)_container.GetService(interf)!);
        }


        return ValueTask.CompletedTask;
    }
}
