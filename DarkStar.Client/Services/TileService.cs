using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DarkStar.Client.Models.Dto;
using DarkStar.Client.Models.Events;
using DarkStar.Client.Utils;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace DarkStar.Client.Services;

public class TileService
{
    private readonly ILogger _logger;
    private readonly ServiceContext _serviceContext;

    public TileService(ILogger<TileService> logger, ServiceContext serviceContext)
    {
        _logger = logger;
        _serviceContext = serviceContext;
        MessageBus.Current.Listen<OnConnectedEvent>()
            .Subscribe(
                @event => { _ = Task.Run(CheckAndDownloadTiles); }
            );
    }


    public async Task CheckAndDownloadTiles()
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(_serviceContext.ServerUrl);
        MessageBus.Current.SendMessage(new ProgressUpdateEvent("Download tilesets..."));
        var tileSets = await httpClient.GetFromJsonAsync<List<TileSetDto>>("/api/tiles/tilesets");
        foreach (var tileSet in tileSets)
        {
            await DownloadTileSet(tileSet);
        }
    }

    private async Task DownloadTileSet(TileSetDto tileSet)
    {
        var fileName = Path.Join(_serviceContext.AssetDirectory, $"{tileSet.Id}.png");
        if (File.Exists(fileName) && new FileInfo(fileName).Length == tileSet.FileSize)
        {
            MessageBus.Current.SendMessage(new ProgressUpdateEvent($"Skipping {tileSet.Name}"));
            return;
        }



        using var httpClient = new HttpClient();
        var tileSetMemoryStream = new MemoryStream();
        httpClient.BaseAddress = new Uri(_serviceContext.ServerUrl);
        MessageBus.Current.SendMessage(new ProgressUpdateEvent($"Downloading {tileSet.Name}"));
        await httpClient.DownloadAsync(
            $"api/tiles/tileset/source/{tileSet.Id}",
            tileSetMemoryStream,
            new Progress<long>(
                f =>
                {
                    MessageBus.Current.SendMessage(
                        new ProgressUpdateEvent($"Downloading {tileSet.Name}", (int)f, (int)tileSet.FileSize)
                    );
                }
            )
        );

        await File.WriteAllBytesAsync(fileName, tileSetMemoryStream.ToArray());
        MessageBus.Current.SendMessage(
            new ProgressUpdateEvent($"{tileSet.Name} downloaded!")
        );
    }
}
