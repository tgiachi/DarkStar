﻿using DarkStar.Network.Protocol.Messages.Common;

namespace DarkStar.Client.Models.Tiles;

public class Tile
{
    public string Id { get; set; }
    public uint TileId { get; set; }
    public PointPosition Position { get; set; }

    public TileOrientation Orientation { get; set; } = TileOrientation.West;

    public Tile(string id, uint tileId, PointPosition position)
    {
        Id = id;
        TileId = tileId;
        Position = position;
    }
}

public enum TileOrientation
{
    North,
    South,
    East,
    West
}