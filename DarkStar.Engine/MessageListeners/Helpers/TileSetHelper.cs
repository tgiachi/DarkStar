using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Database.Entities.TileSets;
using DarkStar.Network.Protocol.Messages.TileSet;

namespace DarkStar.Engine.MessageListeners.Helpers
{
    public static class TileSetHelper
    {
        private static readonly Dictionary<string, byte[]> s_tileSetImageCache = new();
        private static readonly Dictionary<string, List<TileSetMapEntry>> s_tileSetMapCache = new();


        public static async ValueTask<byte[]> GetTileSetAsync(string tileId, IDarkSunEngine engine)
        {
            if (s_tileSetImageCache.TryGetValue(tileId, out var fileContent))
            {
                return fileContent;
            }

            var tileSet = await engine.DatabaseService.QueryAsSingleAsync<TileSetEntity>(x => x.Name == tileId);
            var tileSetImage = await File.ReadAllBytesAsync(tileSet.Source);

            s_tileSetImageCache.Add(tileId, tileSetImage);

            return tileSetImage;

        }

        public static async ValueTask<List<TileSetMapEntry>> GetTileSetMapAsync(string tileId, IDarkSunEngine engine)
        {
            if (s_tileSetMapCache.TryGetValue(tileId, out var tileSetMapE))
            {
                return tileSetMapE;
            }

            var tileSet = await engine.DatabaseService.QueryAsSingleAsync<TileSetEntity>(x => x.Name == tileId);
            var tileSetMap = await engine.DatabaseService.QueryAsListAsync<TileSetMapEntity>(x => x.TileSetId == tileSet.Id);
            var list = tileSetMap.Select(x => new TileSetMapEntry(x.TileType, x.TileId)).ToList();
            s_tileSetMapCache.Add(tileId, list);
            return list;
        }

        public static async ValueTask<(byte[], List<TileSetMapEntry>)> GetTileSetContentAndMapAsync(string tileSet,
            IDarkSunEngine engine)
        {
            return (await GetTileSetAsync(tileSet, engine), await GetTileSetMapAsync(tileSet, engine));

        }
    }
}
