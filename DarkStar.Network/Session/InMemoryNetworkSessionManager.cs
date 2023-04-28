using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Session.Data;
using DarkStar.Network.Session.Interfaces;

namespace DarkStar.Network.Session;

public class InMemoryNetworkSessionManager : INetworkSessionManager
{
    private readonly SemaphoreSlim _sessionLock = new(1);
    private readonly Dictionary<string, DarkSunSession> _sessions = new();


    public string AddSession(string? sessionGuid)
    {
        _sessionLock.Wait();
        sessionGuid ??= Guid.NewGuid().ToString();

        _sessions.Add(sessionGuid, new DarkSunSession { SessionId = sessionGuid });
        _sessionLock.Release();

        return sessionGuid;
    }

    public bool RemoveSession(string sessionGuid)
    {
        _sessionLock.Wait();
        _sessions.Remove(sessionGuid);
        _sessionLock.Release();

        return true;
    }

    public DarkSunSession GetSession(string sessionGuid)
    {
        if (_sessions.TryGetValue(sessionGuid, out var session))
        {
            return session;
        }

        throw new Exception($"Session {sessionGuid} not exists!");
    }
}
