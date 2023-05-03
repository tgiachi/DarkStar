using DarkStar.Engine.Http.Controllers;
using DarkStar.Network.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace DarkStar.Engine.Http;

public static class WebModuleLoaderExtension
{
    public static IServiceCollection ConfigureWebServer(this IServiceCollection services)
    {
        services
            .AddRouting()
            .AddControllers()
            .ConfigureApplicationPartManager(
                manager => { manager.ApplicationParts.Add(new AssemblyPart(typeof(VersionController).Assembly)); }
            );


        services.AddSignalR();


        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        return services;
    }

    public static IApplicationBuilder ConfigureWebServerApp(this IApplicationBuilder builder, string httpRootDirectory)
    {
        var defaultFileOptions = new DefaultFilesOptions();
        defaultFileOptions.DefaultFileNames.Clear();
        defaultFileOptions.DefaultFileNames.Add("index.html");
        defaultFileOptions.DefaultFileNames.Add("index.htm");


        builder
            .UseRouting()
            .UseSwagger()
            .UseSwaggerUI()
            .UseDefaultFiles(defaultFileOptions)
            .UseStaticFiles(
                new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(httpRootDirectory)
                }
            )
            .UseEndpoints(
                routeBuilder =>
                {
                    routeBuilder.MapHub<SignalrMessageHub>("/messages");
                    routeBuilder.MapControllers();
                }
            );
        return builder;
    }
}
