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

    // Account messages
    AccountLoginRequest,
    AccountLoginResponse,
    AccountCreateRequest,
    AccountCreateResponse,

    PlayerRacesRequest,
    PlayerRacesResponse,
    PlayerListRequest,
    PlayerListResponse,
    PlayerCreateRequest,
    PlayerCreateResponse,
    PlayerSelectRequest,
    PlayerSelectResponse,

    MapRequest,
    MapResponse,
}
