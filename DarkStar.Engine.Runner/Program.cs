using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DarkStar.Api.Data.Config;
using DarkStar.Api.Engine.Data.Config;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Utils;
using DarkStar.Engine.Http;
using DarkStar.Engine.Utils;
using DarkStar.Network.Client;
using DarkStar.Network.Client.Interfaces;
using DarkStar.Network.Data;
using DarkStar.Network.Protocol.Builders;
using DarkStar.Network.Protocol.Interfaces.Builders;
using DarkStar.Network.Server;
using DarkStar.Network.Server.Interfaces;
using DarkStar.Network.Session;
using DarkStar.Network.Session.Interfaces;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Redbus;
using Redbus.Configuration;
using Redbus.Interfaces;
using Serilog;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


namespace DarkStar.Engine.Runner;

internal class Program
{
    private static readonly ISerializer s_yamlSerializer = new SerializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .Build();

    private static readonly IDeserializer s_yamlDeserializer = new DeserializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .Build();

    public static readonly Stopwatch StartupStopwatch = Stopwatch.StartNew();

    private static async Task Main(string[] args)
    {
        AssemblyUtils.AddAssembly(typeof(DarkSunEngine).Assembly);

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        var directoryConfig = EnsureDirectories();
        var engineConfig = LoadConfig(directoryConfig);

        if (engineConfig.Logger.EnableDebug)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
        }

        foreach (var assembly in engineConfig.Assemblies.AssemblyNames)
        {
            try
            {
                AssemblyUtils.AddAssembly(Assembly.GetAssembly(AssemblyUtils.GetType(assembly)!)!);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to load assembly: {Assembly}", assembly);
                throw new Exception($"Failed to load assembly: {assembly}", ex);
            }
        }

        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddSingleton<IDarkSunEngine, DarkSunEngine>()
                    .AddSingleton<IDarkSunNetworkServer, TcpNetworkServer>()
                    .AddSingleton<INetworkSessionManager, InMemoryNetworkSessionManager>()
                    .AddSingleton<INetworkMessageBuilder, ProtoBufMessageBuilder>()
                    .AddSingleton<IEventBus>(new EventBus(new EventBusConfiguration()
                    {
                        ThrowSubscriberException = true
                    }))
                    .AddSingleton(new DarkStarNetworkServerConfig()
                    {
                        Address = engineConfig.NetworkServer.Address,
                        Port = engineConfig.NetworkServer.Port
                    })
                    // Only for test
                    .AddSingleton(new DarkStarNetworkClientConfig())
                    .AddSingleton<IDarkSunNetworkClient, TcpNetworkClient>()
                    .AddSingleton(engineConfig)
                    .AddSingleton(directoryConfig)
                    .RegisterDarkSunServices()
                    .RegisterMessageListeners()
                    .RegisterCommandExecutors()
                    .RegisterScriptEngineFunctions()
                    .AddHostedService<DarkSunEngineHostedService>();

                if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOCKER_CONTAINER")))
                {
                    services.AddHostedService<DarkSunTerminalHostedService>();
                }

                if (engineConfig.HttpServer.Enabled)
                {
                    services.ConfigureWebServer();
                }

            })
            .UseSerilog()
            .UseConsoleLifetime()
            .ConfigureWebHostDefaults(builder =>
            {
                if (!engineConfig.HttpServer.Enabled)
                {
                    return;
                }

                Log.Logger.Information("Starting HTTP server - http root Directory is: {RootDirectory}", directoryConfig[DirectoryNameType.HttpRoot]);
                builder.Configure(applicationBuilder =>
                {
                    applicationBuilder.ConfigureWebServerApp(directoryConfig[DirectoryNameType.HttpRoot]);
                });

            })

            .Build();

        await host.RunAsync();
        Log.Logger.Information("Engine has stopped");
    }

    private static DirectoriesConfig EnsureDirectories()
    {
        var rootDirectory = Environment.GetEnvironmentVariable("DARKSTAR_ROOT_DIRECTORY")
                            ?? Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                "DarkStar");

        if (string.IsNullOrEmpty(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)))
        {
            rootDirectory = Path.Join(Directory.GetCurrentDirectory(), "DarkStar");
        }

        Log.Logger.Information("Root directory is: {Root}", rootDirectory);

        var directoriesConfig = new DirectoriesConfig { [DirectoryNameType.Root] = rootDirectory };

        Directory.CreateDirectory(directoriesConfig[DirectoryNameType.Root]);

        foreach (var type in Enum.GetValues(typeof(DirectoryNameType)).Cast<DirectoryNameType>())
        {
            if (type == DirectoryNameType.Root)
            {
                continue;
            }

            directoriesConfig[type] = Path.Join(directoriesConfig[DirectoryNameType.Root], type.ToString());
            Log.Logger.Debug("{Type} directory is: {Directory}", type, directoriesConfig[type]);
            Directory.CreateDirectory(directoriesConfig[type]);
        }

        return directoriesConfig;
    }

    private static EngineConfig LoadConfig(DirectoriesConfig directoriesConfig)
    {


        var config = new EngineConfig();
        var configPath = Path.Join(directoriesConfig[DirectoryNameType.Config], "DarkStar.yml");
        if (File.Exists(configPath))
        {
            Log.Logger.Information("Loading config from {Path}", configPath);

            var source = File.ReadAllText(configPath);
            config = s_yamlDeserializer.Deserialize<EngineConfig>(source);
        }
        else
        {
            Log.Logger.Information("Creating default config at {Path}", configPath);
            var source = s_yamlSerializer.Serialize(config);
            File.WriteAllText(configPath, source);
        }

        return config!;
    }
}
