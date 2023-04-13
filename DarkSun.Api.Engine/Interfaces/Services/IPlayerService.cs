using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Data.Sessions;
using DarkSun.Api.Engine.Interfaces.Services.Base;
using DarkSun.Api.World.Types.Tiles;
using DarkSun.Database.Entities.Base;
using DarkSun.Database.Entities.Player;

namespace DarkSun.Api.Engine.Interfaces.Services;

public interface IPlayerService : IDarkSunEngineService
{
    void AddSession(Guid networkSessionId);

    void RemoveSession(Guid networkSessionId);

    PlayerSession GetSession(Guid networkSessionId);

    Task<List<PlayerEntity>> GetPlayersByAccountIdAsync(Guid accountId);

    Task<PlayerEntity> CreatePlayerAsync(Guid accountId, TileType tileId, Guid raceId, BaseStatEntity stats);
}
