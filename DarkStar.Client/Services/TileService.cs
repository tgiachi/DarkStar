using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.OpenGL.Imaging;
using DarkStar.Api.Utils;
using DarkStar.Client.Models.Dto;
using DarkStar.Client.Models.Events;
using DarkStar.Client.Utils;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using SkiaSharp;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Size = Avalonia.Size;

namespace DarkStar.Client.Services;

public class TileService
{
    public bool TilesReady { get; set; }
    private readonly ILogger _logger;
    private readonly ServiceContext _serviceContext;

    private IImage _defaultTileSet;
    private SKBitmap _defaultSkImageTileSet;
    public int TileWidth { get; set; }
    public int TileHeight { get; set; }
    private int _imageWidth;
    private int _imageHeight;

    public TileService(ILogger<TileService> logger, ServiceContext serviceContext)
    {
        _logger = logger;
        _serviceContext = serviceContext;
        MessageBus.Current.Listen<OnConnectedEvent>()
            .Subscribe(
                @event => { _ = Task.Run(CheckAndDownloadTiles); }
            );
    }

    public CroppedBitmap GetTileId(int tileId)
    {
        if (tileId == 0)
        {
            tileId = RandomUtils.Range(100, 4000);
        }

        var x = tileId % (_imageWidth / TileWidth);
        var y = tileId / (_imageHeight / TileHeight);

        var cropped = new CroppedBitmap(
            _defaultTileSet,
            new PixelRect(x * TileWidth, y * TileHeight, TileWidth, TileHeight)
        );

        return cropped;
    }

    public SKBitmap GetSkImageTile(int tileId)
    {
        // if (tileId == 0)
        // {
        //     tileId = RandomUtils.Range(100, 4000);
        // }

        var x = tileId % (_imageWidth / TileWidth);
        var y = tileId / (_imageHeight / TileHeight);


        var cropped = new SKBitmap(TileWidth, TileHeight);
        _defaultSkImageTileSet.ExtractSubset(cropped, SKRectI.Create(x * TileWidth, y * TileHeight, TileWidth, TileWidth));

        return cropped;
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
        TilesReady = true;
    }

    private async Task DownloadTileSet(TileSetDto tileSet)
    {
        var fileName = Path.Join(_serviceContext.AssetDirectory, $"{tileSet.Id}.png");
        if (File.Exists(fileName) && new FileInfo(fileName).Length == tileSet.FileSize)
        {
            MessageBus.Current.SendMessage(new ProgressUpdateEvent($"Skipping {tileSet.Name}"));
            _defaultSkImageTileSet = SKBitmap.Decode(fileName);
            using var tileImage = Image.FromFile(fileName);
            _defaultTileSet = new Bitmap(fileName);


            TileHeight = tileSet.TileHeight;
            TileWidth = tileSet.TileWidth;
            _imageWidth = tileImage.Width;
            _imageHeight = tileImage.Height;
            TilesReady = true;
            MessageBus.Current.SendMessage(new TilesReadyEvent());
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
        using var image = Image.FromStream(tileSetMemoryStream);
        _defaultTileSet = new Bitmap(tileSetMemoryStream);
        _defaultSkImageTileSet = SKBitmap.Decode(tileSetMemoryStream);
        TileHeight = tileSet.TileHeight;
        TileWidth = tileSet.TileWidth;
        _imageWidth = image.Width;
        _imageHeight = image.Height;

        MessageBus.Current.SendMessage(
            new ProgressUpdateEvent($"{tileSet.Name} downloaded!")
        );
        MessageBus.Current.SendMessage(new TilesReadyEvent());
        TilesReady = true;

    }
}
