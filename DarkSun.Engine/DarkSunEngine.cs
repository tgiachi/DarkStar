﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Engine.Attributes.Network;
using DarkStar.Api.Engine.Events.Engine;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Interfaces.Listener;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.Interfaces.Services.Base;
using DarkStar.Api.Utils;
using DarkStar.Network.Client.Interfaces;
using DarkStar.Network.Interfaces;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Live;
using DarkStar.Network.Protocol.Messages.Accounts;
using DarkStar.Network.Server.Interfaces;

using Microsoft.Extensions.Logging;
using Redbus.Interfaces;

namespace DarkStar.Engine
{
    public class DarkSunEngine : IDarkSunEngine
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _container;
        private readonly SortedDictionary<int, HashSet<IDarkSunEngineService>> _servicesLoadOrder = new();
        private readonly HashSet<INetworkConnectionHandler> _connectionHandlers = new();

        //Only for test
        private readonly IDarkSunNetworkClient _networkClient;

        public string ServerName { get; set; } = null!;
        public string ServerMotd { get; set; } = null!;

        public IWorldService WorldService { get; }
        public IBlueprintService BlueprintService { get; }
        public ISchedulerService SchedulerService { get; }
        public IScriptEngineService ScriptEngineService { get; }
        public IDarkSunNetworkServer NetworkServer { get; }
        public IPlayerService PlayerService { get; }
        public IDatabaseService DatabaseService { get; }
        public ICommandService CommandService { get; }
        public INamesService NamesService { get; }
        public ISeedService SeedService { get; }
        public IJobSchedulerService JobSchedulerService { get; }
        public IItemService ItemService { get; }
        public IEventBus EventBus { get; }

        public DarkSunEngine(ILogger<DarkSunEngine> logger,
            IBlueprintService blueprintService,
            ISchedulerService schedulerService,
            IScriptEngineService scriptEngineService,
            IDarkSunNetworkServer networkServer,
            IWorldService worldService,
            IPlayerService playerService,
            IDatabaseService databaseService,
            ICommandService commandService,
            IServiceProvider container,
            IEventBus eventBus,
            IDarkSunNetworkClient networkClient,
            INamesService namesService,
            ISeedService seedService,
            IJobSchedulerService jobSchedulerService,
            IItemService itemService)
        {
            _logger = logger;
            WorldService = worldService;
            BlueprintService = blueprintService;
            SchedulerService = schedulerService;
            ScriptEngineService = scriptEngineService;
            NetworkServer = networkServer;
            PlayerService = playerService;
            DatabaseService = databaseService;
            EventBus = eventBus;
            _networkClient = networkClient;
            NamesService = namesService;
            SeedService = seedService;
            JobSchedulerService = jobSchedulerService;
            ItemService = itemService;
            CommandService = commandService;
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
                if (_container.GetService(messageListenerType) is INetworkServerMessageListener service)
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

            foreach (var services in _servicesLoadOrder)
            {
                foreach (var service in services.Value)
                {
                    await service.StartAsync(this);
                }
            }

            await NetworkServer.StartAsync();
            JobSchedulerService.AddJob("PingClients", () =>
            {
                _ = Task.Run(() => NetworkServer.BroadcastMessageAsync(new PingMessageResponse { TimeStamp = DateTime.Now.Ticks }));

            }, (int)TimeSpan.FromMinutes(5).TotalSeconds, false);

            _ = Task.Run(async () =>
            {
                await _networkClient.ConnectAsync();
                await _networkClient.SendMessageAsync(new AccountCreateRequestMessage()
                {
                    Email = "test@test.com",
                    Password = "1234"
                });
                await _networkClient.SendMessageAsync(new AccountLoginRequestMessage("test@test.com", "12345"));
                await _networkClient.SendMessageAsync(new AccountLoginRequestMessage("test@test.com", "1234"));
                //await _networkClient.DisconnectAsync();
            });

            EventBus.PublishAsync(new EngineReadyEvent());

            return true;
        }

        public async ValueTask<bool> StopAsync()
        {
            await NetworkServer.StopAsync();
            foreach (var services in _servicesLoadOrder.Reverse())
            {
                foreach (var service in services.Value)
                {
                    await service.StopAsync();
                }
            }

            return true;
        }

        private ValueTask BuildServicesOrderAsync()
        {
            _logger.LogDebug("Building services load order");
            var services = AssemblyUtils.GetAttribute<DarkStarEngineServiceAttribute>();
            foreach (var serviceType in services)
            {
                var attr = serviceType.GetCustomAttribute<DarkStarEngineServiceAttribute>()!;
                var interf =
                    AssemblyUtils.GetInterfacesOfType(serviceType)!.First(k => k.Name.EndsWith(serviceType.Name));
                if (_servicesLoadOrder.TryGetValue(attr.LoadOrder, out var value))
                {
                    value.Add((IDarkSunEngineService)_container.GetService(interf)!);
                }
                else
                {
                    _servicesLoadOrder.Add(attr.LoadOrder, new HashSet<IDarkSunEngineService>());
                    _servicesLoadOrder[attr.LoadOrder].Add((IDarkSunEngineService)_container.GetService(interf)!);
                }
            }


            return ValueTask.CompletedTask;
        }
    }
}
