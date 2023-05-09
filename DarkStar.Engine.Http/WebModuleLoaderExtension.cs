using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DarkStar.Api.Utils;
using DarkStar.Engine.Http.Controllers;
using DarkStar.Network.Attributes;
using DarkStar.Network.Hubs;
using DarkStar.Network.Protocol.Live;
using DarkStar.Network.Protocol.Messages.Players;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using ProtoBuf;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DarkStar.Engine.Http;

public static class WebModuleLoaderExtension
{
    public static IServiceCollection ConfigureWebServer(this IServiceCollection services)
    {
        services
            .AddRouting()
            .AddControllers()
            .AddJsonOptions(
                options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                }
            )
            .ConfigureApplicationPartManager(
                manager => { manager.ApplicationParts.Add(new AssemblyPart(typeof(VersionController).Assembly)); }
            );


        services.AddSignalR();


        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(
                options =>
                {
                    options.SchemaFilter<EnumSchemaFilter>();
                    options.DocumentFilter<NetworkMessageDocumentFilter>();
                    options.SwaggerDoc("v1", new OpenApiInfo() { Title = "DarkStar", Version = Assembly.GetExecutingAssembly().GetName().Version.ToString() });
                }
            );


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

public class NetworkMessageDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        AssemblyUtils.GetAttribute<ProtoContractAttribute>()
            .ForEach(
                s => { context.SchemaGenerator.GenerateSchema(s, context.SchemaRepository); }
            );
    }
}

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum.Clear();
            Enum.GetNames(context.Type)
                .ToList()
                .ForEach(name => schema.Enum.Add(new OpenApiString($"{name}")));
        }
    }
}
