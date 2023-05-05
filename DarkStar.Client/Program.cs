using Avalonia;
using Avalonia.ReactiveUI;
using System;
using DarkStar.Client.Services;
using DarkStar.Client.ViewModels;
using DarkStar.Network.Client;
using DarkStar.Network.Client.Interfaces;
using DarkStar.Network.Data;
using DarkStar.Network.Protocol.Builders;
using DarkStar.Network.Protocol.Interfaces.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Serilog;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace DarkStar.Client;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args){

        InitializeDependencyInjection();
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    private static void InitializeDependencyInjection()
    {
        ServiceCollection services = new();

        ConfigureServices(services);

        services.UseMicrosoftDependencyResolver();

        Locator.CurrentMutable.InitializeSplat();
        Locator.CurrentMutable.InitializeReactiveUI();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<INetworkMessageBuilder, JsonMessageBuilder>();
        services.AddSingleton<IDarkStarNetworkClient, SignalrNetworkClient>();
        services.AddSingleton(new DarkStarNetworkClientConfig());
        services.AddSingleton<MainWindowViewModel>();

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
            builder.AddSerilog(dispose: true);
        });

    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp() =>
        // Preparing context
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
}
