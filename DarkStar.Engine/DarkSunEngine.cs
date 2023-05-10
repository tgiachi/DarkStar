using System.Reflection;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Engine.Attributes.Network;
using DarkStar.Api.Engine.Events.Engine;
using DarkStar.Api.Engine.Events.Players;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Interfaces.Listener;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.Interfaces.Services.Base;
using DarkStar.Api.Utils;
using DarkStar.Database.Entities.Races;
using DarkStar.Network.Client.Interfaces;
using DarkStar.Network.Interfaces;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Accounts;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Messages.Players;
using DarkStar.Network.Protocol.Messages.TileSet;
using DarkStar.Network.Server.Interfaces;
using Microsoft.Extensions.Logging;
using Redbus.Interfaces;

namespace DarkStar.Engine;

public class DarkSunEngine : IDarkSunEngine
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _container;
    private readonly SortedDictionary<int, HashSet<IDarkSunEngineService>> _servicesLoadOrder = new();
    private readonly HashSet<INetworkConnectionHandler> _connectionHandlers = new();

    //Only for test
    private readonly IDarkStarNetworkClient _networkClient;

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
    public ITypeService TypeService { get; }

    public DarkSunEngine(
        ILogger<DarkSunEngine> logger,
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
        IDarkStarNetworkClient networkClient,
        INamesService namesService,
        ISeedService seedService,
        IJobSchedulerService jobSchedulerService,
        IItemService itemService,
        ITypeService typeService
    )
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
        TypeService = typeService;
        CommandService = commandService;
        _container = container;
    }

    private ValueTask PrepareMessageListenersAsync()
    {
        var messageListenersTypes = AssemblyUtils.GetAttribute<NetworkMessageListenerAttribute>();
        foreach (var messageListenerType in messageListenersTypes)
        {
            var attribute = messageListenerType.GetCustomAttribute<NetworkMessageListenerAttribute>()!;
            _logger.LogDebug(
                "Adding message listener {GameObjectType} from message type: {MessageType}",
                messageListenerType.Name,
                attribute.MessageType
            );
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

    private async Task NetworkServerOnClientDisconnectedAsync(string sessionId)
    {
        foreach (var handler in _connectionHandlers)
        {
            await handler.ClientDisconnectedAsync(sessionId);
        }
    }

    private async Task<List<IDarkStarNetworkMessage>> NetworkServerOnOnClientConnectedAsync(string sessionId)
    {
        var messages = new List<IDarkStarNetworkMessage>();
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
        JobSchedulerService.AddJob(
            "PingClients",
            () => { EventBus.PublishAsync(new PingRequestEvent()); },
            (int)TimeSpan.FromMinutes(5).TotalSeconds,
            false
        );

        // TODO: This is only for testing, will be removed later

        EventBus.Subscribe<EngineReadyEvent>(OnEngineReady);
        EventBus.PublishAsync(new EngineReadyEvent());

        return true;
    }

    private void OnEngineReady(EngineReadyEvent obj)
    {
        _ = Task.Run(
            async () =>
            {
                await _networkClient.ConnectAsync();

                var race = new RaceEntity
                {
                    TileId = 0,
                    Dexterity = 0,
                    Health = 0,
                    IsVisible = true,
                    Luck = 0,
                    Strength = 0,
                    Name = "Humans"
                };
                await DatabaseService.InsertAsync(race);

                await _networkClient.SendMessageAsync(
                    new AccountCreateRequestMessage()
                    {
                        Email = "test@test.com",
                        Password = "1234"
                    }
                );
                await _networkClient.SendMessageAsync(new AccountLoginRequestMessage("test@test.com", "12345"));
                await _networkClient.SendMessageAsync(new AccountLoginRequestMessage("test@test.com", "1234"));
                await _networkClient.SendMessageAsync(new TileSetListRequestMessage());
                await _networkClient.SendMessageAsync(new TileSetDownloadRequestMessage("Tangaria"));
                await _networkClient.SendMessageAsync(new TileSetMapRequestMessage("Tangaria"));
                await _networkClient.SendMessageAsync(
                    new PlayerCreateRequestMessage()
                    {
                        Dexterity = 10,
                        Intelligence = 10,
                        Luck = 10,
                        Name = "Player 1",
                        Strength = 10,
                        TileId = 1416,
                        RaceId = race.Id
                    }
                );
                await _networkClient.SendMessageAsync(new PlayerLoginRequestMessage(Guid.Empty, "Player 1"));
                foreach (var _ in Enumerable.Range(0, 3))
                {
                    await Task.Delay(1000);
                    await _networkClient.SendMessageAsync(
                        new PlayerMoveRequestMessage(MoveDirectionType.North.RandomEnumValue())
                    );
                }

                await _networkClient.SendMessageAsync(new PlayerLogoutRequestMessage());
                await _networkClient.DisconnectAsync();
            }
        );
    }

    public async ValueTask<bool> StopAsync()
    {
        EventBus.PublishAsync(new EngineStoppingEvent());

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
