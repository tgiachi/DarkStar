namespace DarkStar.Engine.Http.Dto;

public class TileSetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public long FileSize { get; set; }
    public int TileWidth { get; set; }
    public int TileHeight { get; set; }
}
