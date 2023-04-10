using DarkSun.Engine.Utils;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DarkSun.Engine.Runner
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.RegisterDarkSunServices();

                })
                .UseSerilog()
                .Build();

            await host.RunAsync();
        }
    }
}
