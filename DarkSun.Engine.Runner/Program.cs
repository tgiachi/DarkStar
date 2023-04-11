using System.Text.Json;
using System.Text.Json.Serialization;
using DarkSun.Api.Data.Config;
using DarkSun.Api.Engine.Data.Config;
using DarkSun.Api.Engine.Interfaces.Core;
using DarkSun.Api.Engine.Interfaces.Services.Base;
using DarkSun.Api.Utils;
using DarkSun.Engine.Utils;
using DarkSun.Network.Data;
using DarkSun.Network.Protocol.Builders;
using DarkSun.Network.Protocol.Interfaces.Builders;
using DarkSun.Network.Server;
using DarkSun.Network.Server.Interfaces;
using DarkSun.Network.Session;
using DarkSun.Network.Session.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Tomlyn;

namespace DarkSun.Engine.Runner
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            AssemblyUtils.AddAssembly(typeof(DarkSunEngine).Assembly);

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            var directoryConfig = EnsureDirectories();
            var engineConfig = LoadConfig(directoryConfig);

            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IDarkSunEngine, DarkSunEngine>()
                    .AddSingleton<IDarkSunNetworkServer, MessagePackNetworkServer>()
                    .AddSingleton<INetworkSessionManager, InMemoryNetworkSessionManager>()
                    .AddSingleton<INetworkMessageBuilder, MessagePackMessageBuilder>()
                    .AddSingleton(new DarkSunNetworkServerConfig()
                    {
                        Address = engineConfig.NetworkServer.Address,
                        Port = engineConfig.NetworkServer.Port
                    })
                    .AddSingleton(engineConfig)
                    .AddSingleton(directoryConfig)
                    .RegisterDarkSunServices()

                    .AddHostedService<DarkEngineHostedService>();

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
                if (type != DirectoryNameType.Root)
                {
                    directoriesConfig[type] = Path.Join(directoriesConfig[DirectoryNameType.Root], type.ToString());
                    Log.Logger.Debug("{Type} directory is: {Directory}", type, directoriesConfig[type]);
                    Directory.CreateDirectory(directoriesConfig[type]);
                }

            return directoriesConfig;
        }

        private static EngineConfig LoadConfig(DirectoriesConfig directoriesConfig)
        {
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };
            var config = new EngineConfig();
            var configPath = Path.Join(directoriesConfig[DirectoryNameType.Config], "darksun.json");
            if (File.Exists(configPath))
            {
                Log.Logger.Information("Loading config from {Path}", configPath);
                var source = File.ReadAllText(configPath);
                config = JsonSerializer.Deserialize<EngineConfig>(source, jsonOptions);
            }
            else
            {
                Log.Logger.Information("Creating default config at {Path}", configPath);
                var source = JsonSerializer.Serialize(config, jsonOptions);
                File.WriteAllText(configPath, source);
            }
            return config!;

        }
    }
}
