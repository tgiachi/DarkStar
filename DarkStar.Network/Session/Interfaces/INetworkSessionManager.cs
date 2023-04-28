using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Session.Data;

namespace DarkStar.Network.Session.Interfaces;

public interface INetworkSessionManager
{
    string AddSession(string? sessionGuid);
    bool RemoveSession(string sessionGuid);
    DarkSunSession GetSession(string sessionGuid);
}
