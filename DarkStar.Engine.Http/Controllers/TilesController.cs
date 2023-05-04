using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Database.Entities.TileSets;
using DarkStar.Engine.Http.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DarkStar.Engine.Http.Controllers;

[Route("api/tiles")]
[ApiController]
public class TilesController : ControllerBase
{
    private readonly IDarkSunEngine _darkSunEngine;

    public TilesController(IDarkSunEngine darkSunEngine) => _darkSunEngine = darkSunEngine;

    [HttpGet]
    [Route("tilesets")]
    public async Task<ActionResult<List<TileSetDto>>> GetTileSets()
    {
        var entities = await _darkSunEngine.DatabaseService.FindAllAsync<TileSetEntity>();

        return Ok(
            entities.Select(
                t => new TileSetDto()
                {
                    Id = t.Id,
                    Name = t.Name,
                    FileSize = t.FileSize,
                    TileWidth = t.TileWidth,
                    TileHeight = t.TileHeight
                }
            )
        );
    }

    [HttpGet]
    [Route("tileset/source/{tileId}")]
    public async Task<IActionResult> GetTileSetSource(Guid tileId)
    {
        var tileSet = await _darkSunEngine.DatabaseService.QueryAsSingleAsync<TileSetEntity>(x => x.Id == tileId);
        var tileSetImage = await System.IO.File.ReadAllBytesAsync(tileSet.Source);

        return File(tileSetImage, "image/png", new FileInfo(tileSet.Source).Name);
    }
}
