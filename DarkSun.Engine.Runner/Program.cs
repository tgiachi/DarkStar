using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DarkSun.Api.Data.Config;
using DarkSun.Api.Engine.Data.Config;
using DarkSun.Api.Engine.Interfaces.Core;
using DarkSun.Api.Engine.Interfaces.Services.Base;
using DarkSun.Api.Utils;
using DarkSun.Engine.Utils;
using DarkSun.Network.Client;
using DarkSun.Network.Client.Interfaces;
using DarkSun.Network.Data;
using DarkSun.Network.Protocol.Builders;
using DarkSun.Network.Protocol.Interfaces.Builders;
using DarkSun.Network.Server;
using DarkSun.Network.Server.Interfaces;
using DarkSun.Network.Session;
using DarkSun.Network.Session.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Redbus;
using Redbus.Configuration;
using Redbus.Interfaces;
using Serilog;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


namespace DarkSun.Engine.Runner;

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
                    .AddSingleton(new DarkSunNetworkServerConfig()
                    {
                        Address = engineConfig.NetworkServer.Address, Port = engineConfig.NetworkServer.Port
                    })
                    // Only for test
                    .AddSingleton(new DarkSunNetworkClientConfig())
                    .AddSingleton<IDarkSunNetworkClient, TcpNetworkClient>()
                    .AddSingleton(engineConfig)
                    .AddSingleton(directoryConfig)
                    .RegisterDarkSunServices()
                    .RegisterMessageListeners()
                    .RegisterScriptEngineFunctions()
                    .AddHostedService<DarkSunEngineHostedService>()
                    .AddHostedService<DarkSunTerminalHostedService>();
            })
            .UseSerilog()
            .Build();

        await host.RunAsync();
        Log.Logger.Information("Engine has stopped");
    }

    private static DirectoriesConfig EnsureDirectories()
    {
        var rootDirectory = Environment.GetEnvironmentVariable("DARKSUN_ROOT_DIRECTORY")
                            ?? Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                "DarkSun");

        Log.Logger.Information("Root directory is: {Root}", rootDirectory);

        var directoriesConfig = new DirectoriesConfig { [DirectoryNameType.Root] = rootDirectory };

        Directory.CreateDirectory(directoriesConfig[DirectoryNameType.Root]);

        foreach (var type in Enum.GetValues(typeof(DirectoryNameType)).Cast<DirectoryNameType>())
        {
            if (type != DirectoryNameType.Root)
            {
                directoriesConfig[type] = Path.Join(directoriesConfig[DirectoryNameType.Root], type.ToString());
                Log.Logger.Debug("{Type} directory is: {Directory}", type, directoriesConfig[type]);
                Directory.CreateDirectory(directoriesConfig[type]);
            }
        }

        return directoriesConfig;
    }

    private static EngineConfig LoadConfig(DirectoriesConfig directoriesConfig)
    {


        var config = new EngineConfig();
        var configPath = Path.Join(directoriesConfig[DirectoryNameType.Config], "darksun.yml");
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
