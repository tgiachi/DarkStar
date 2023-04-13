using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Session.Data;
using DarkSun.Network.Session.Interfaces;

namespace DarkSun.Network.Session;

public class InMemoryNetworkSessionManager : INetworkSessionManager
{
    private readonly SemaphoreSlim _sessionLock = new(1);
    private readonly Dictionary<Guid, DarkSunSession> _sessions = new();


    public Guid AddSession(Guid? sessionGuid)
    {
        _sessionLock.Wait();
        sessionGuid ??= Guid.NewGuid();

        _sessions.Add(sessionGuid.Value, new DarkSunSession { SessionId = sessionGuid.Value });
        _sessionLock.Release();

        return sessionGuid.Value;
    }

    public bool RemoveSession(Guid sessionGuid)
    {
        _sessionLock.Wait();
        _sessions.Remove(sessionGuid);
        _sessionLock.Release();

        return true;
    }

    public DarkSunSession GetSession(Guid sessionGuid)
    {
        if (_sessions.TryGetValue(sessionGuid, out var session))
        {
            return session;
        }

        throw new Exception($"Session {sessionGuid} not exists!");
    }
}
