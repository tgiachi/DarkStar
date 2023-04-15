using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Data.Sessions;
using DarkStar.Api.Engine.Interfaces.Services.Base;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Database.Entities.Base;
using DarkStar.Database.Entities.Player;

namespace DarkStar.Api.Engine.Interfaces.Services;

public interface IPlayerService : IDarkSunEngineService
{
    void AddSession(Guid networkSessionId);

    void RemoveSession(Guid networkSessionId);

    PlayerSession GetSession(Guid networkSessionId);

    Task<List<PlayerEntity>> GetPlayersByAccountIdAsync(Guid accountId);

    Task<PlayerEntity> CreatePlayerAsync(Guid accountId, TileType tileId, Guid raceId, BaseStatEntity stats);
}
