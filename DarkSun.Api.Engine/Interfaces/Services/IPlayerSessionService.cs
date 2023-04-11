using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Data.Sessions;
using DarkSun.Api.Engine.Interfaces.Services.Base;

namespace DarkSun.Api.Engine.Interfaces.Services;

public interface IPlayerSessionService : IDarkSunEngineService
{
    void AddPlayerSession(Guid networkSessionId);

    void RemovePlayerSession(Guid networkSessionId);

    PlayerSession GetPlayerSession(Guid networkSessionId);
}
