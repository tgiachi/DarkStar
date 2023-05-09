using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DarkStar.Api.World.Types.Map;
using DarkStar.Client.Models.Tiles;
using DarkStar.Network.Protocol.Messages.Common;
using SkiaSharp;

namespace DarkStar.Client.Services;

public class GraphicEngineRender
{
    private readonly TileService _tileService;
    private readonly SemaphoreSlim _layerLock = new(1);
    private readonly Dictionary<MapLayer, List<Tile>> _layers = new();


    public Action<(SKCanvas canvas, TimeSpan delta)> RenderAction { get; set; } = null!;


    public GraphicEngineRender(TileService tileService)
    {
        _tileService = tileService;
        ClearLayers();
        RenderAction = OnRenderAction;

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                AddTile(MapLayer.Terrain, new Tile(Guid.NewGuid().ToString(), 6272, new PointPosition(x, y)));
            }
        }

        AddTile(MapLayer.Creatures, new Tile(Guid.NewGuid().ToString(), 3479, new PointPosition(5, 5)));
    }

    public void ClearLayers()
    {
        _layerLock.Wait();
        _layers.Clear();

        foreach (var layer in Enum.GetValues<MapLayer>())
        {
            _layers.Add(layer, new List<Tile>());
        }

        _layerLock.Release();
    }

    public void MoveTile(MapLayer layer, string id, PointPosition newPosition)
    {
        _layerLock.Wait();
        var tiles = _layers[layer];
        var tile = tiles.FirstOrDefault(s => s.Id == id);
        if (tile != null)
        {
            tile.Position = newPosition;
        }
        _layerLock.Release();

    }
    public void AddTile(MapLayer layer, Tile tile)
    {
        _layerLock.Wait();
        _layers[layer].Add(tile);
        _layerLock.Release();
    }

    private void OnRenderAction((SKCanvas canvas, TimeSpan delta) obj)
    {
        var canvas = obj.canvas;
        var delta = obj.delta;

        canvas.Clear(SKColors.Black);

        canvas.DrawText(
            "ciao",
            new SKPoint(10, 10),
            new SKPaint
            {
                Typeface = SKTypeface.Default,
                Color = SKColors.Cornsilk
            }
        );

        _layerLock.Wait();
        foreach (var layer in _layers)
        {
            foreach (var tile in layer.Value)
            {
                RenderTile(tile, canvas, delta);
            }
        }

        _layerLock.Release();
    }

    private void RenderTile(Tile tile, SKCanvas canvas, TimeSpan delta)
    {
        canvas.DrawBitmap(
            _tileService.GetSkImageTile((int)tile.TileId),
            new SKPoint(tile.Position.X * _tileService.TileWidth, tile.Position.Y * _tileService.TileHeight)
        );
    }
}
