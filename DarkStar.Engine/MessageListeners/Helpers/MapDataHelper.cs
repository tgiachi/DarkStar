using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.World.Types.Map;
using DarkStar.Network.Protocol.Map;
using DarkStar.Network.Protocol.Messages.Common;

namespace DarkStar.Engine.MessageListeners.Helpers;

public class MapDataHelper
{
    public static async Task<MapResponseMessage> BuildMapResponseDataAsync(IDarkSunEngine engine, string mapId, Guid playerId)
    {
        var map = engine.WorldService.GetMap(mapId);
        var message = new MapResponseMessage
        {
            MapId = mapId,
            Height = map.Height,
            Width = map.Width,
            Name = engine.WorldService.GetMapName(mapId),
            MapType = engine.WorldService.GetMapType(mapId),
        };

        message.TerrainsLayer = (await engine.WorldService.GetAllEntitiesInLayerAsync<TerrainGameObject>(mapId, MapLayer.Terrain)).Select(
            o => new MapEntityNetworkObject((int)o.ID, o.ObjectId, o.Tile, new PointPosition(o.Position.X, o.Position.Y))).ToList();

        message.GameObjectsLayer = (await engine.WorldService.GetAllEntitiesInLayerAsync<WorldGameObject>(mapId, MapLayer.Objects)).Select(
            o => new MapEntityNetworkObject((int)o.ID, o.ObjectId, o.Tile, new PointPosition(o.Position.X, o.Position.Y))).ToList();

        message.NpcsLayer = (await engine.WorldService.GetAllEntitiesInLayerAsync<NpcGameObject>(mapId, MapLayer.Creatures)).Select(
            o => new NamedMapEntityNetworkObject((int)o.ID, o.ObjectId, o.Tile, new PointPosition(o.Position.X, o.Position.Y), o.Name)).ToList();

        message.PlayersLayer =
            (await engine.WorldService.GetAllEntitiesInLayerAsync<PlayerGameObject>(mapId, MapLayer.Players)).Select(
                o => new NamedMapEntityNetworkObject(
                    (int)o.ID,
                    o.ObjectId,
                    o.Tile,
                    new PointPosition(o.Position.X, o.Position.Y),
                    o.Name
                )
            ).Where(o => o.ObjectId != playerId).ToList();

        message.ItemsLayer = (await engine.WorldService.GetAllEntitiesInLayerAsync<ItemGameObject>(mapId, MapLayer.Items)).Select(
            o => new NamedMapEntityNetworkObject((int)o.ID, o.ObjectId, o.Tile, new PointPosition(o.Position.X, o.Position.Y), o.Name)).ToList();


        return message;
    }
}
