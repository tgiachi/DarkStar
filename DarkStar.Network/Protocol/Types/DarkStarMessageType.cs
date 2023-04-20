using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Network.Protocol.Types;

public enum DarkStarMessageType : short
{
    Ping,
    Pong,
    // Server messages
    ServerVersionResponse,
    ServerMotdResponse,
    ServerNameResponse,
    ServerMessageResponse,

    // Account messages
    AccountLoginRequest,
    AccountLoginResponse,
    AccountCreateRequest,
    AccountCreateResponse,

    // Player messages
    PlayerRacesRequest,
    PlayerRacesResponse,
    PlayerListRequest,
    PlayerListResponse,
    PlayerCreateRequest,
    PlayerCreateResponse,
    PlayerSelectRequest,
    PlayerSelectResponse,
    PlayerLoginRequest,
    PlayerLoginResponse,
    PlayerLogoutRequest,
    PlayerLogoutResponse,
    PlayerDataRequest,
    PlayerDataResponse,
    PlayerMoveRequest,
    PlayerMoveResponse,

    // TileSet message
    TileSetListRequest,
    TileSetListResponse,
    TileSetDownloadRequest,
    TileSetDownloadResponse,
    TileSetMapRequest,
    TileSetMapResponse,

    // Trigger message
    NpcAddedResponse,
    NpcRemovedResponse,
    NpcMovedResponse,

    ItemAddedResponse,
    ItemRemovedResponse,
    ItemMovedResponse,

    WorldGameObjectAddedResponse,
    WorldGameObjectRemovedResponse,
    WorldGameObjectMovedResponse,

    WorldMessageRequest,
    WorldMessageResponse,

    MapRequest,
    MapResponse,
}
