using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Session.Data;

namespace DarkStar.Network.Session.Interfaces;

public interface INetworkSessionManager
{
    Guid AddSession(Guid? sessionGuid);
    bool RemoveSession(Guid sessionGuid);
    DarkSunSession GetSession(Guid sessionGuid);
}
