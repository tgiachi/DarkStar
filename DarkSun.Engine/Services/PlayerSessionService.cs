using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Engine.Data.Sessions;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Engine.Services.Base;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.Services;

[DarkSunEngineService("PlayerSessionService", 6)]
public class PlayerSessionService : BaseService<PlayerSessionService>, IPlayerSessionService
{
    private readonly Dictionary<Guid, PlayerSession> _playerSessions = new();

    public PlayerSessionService(ILogger<PlayerSessionService> logger) : base(logger)
    {
    }

    public void AddPlayerSession(Guid networkSessionId)
    {
        _playerSessions.Add(networkSessionId, new PlayerSession());
    }

    public void RemovePlayerSession(Guid networkSessionId)
    {
        _playerSessions.Remove(networkSessionId);
    }

    public PlayerSession GetPlayerSession(Guid networkSessionId)
    {
        if (_playerSessions.TryGetValue(networkSessionId, out var session))
        {
            return session;
        }

        throw new Exception($"Can't find network sessionId {networkSessionId}");
    }
}
