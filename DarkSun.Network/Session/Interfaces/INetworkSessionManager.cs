using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Session.Data;

namespace DarkSun.Network.Session.Interfaces
{
    public interface INetworkSessionManager
    {
        Guid AddSession(Guid? sessionGuid);
        bool RemoveSession(Guid sessionGuid);
        DarkSunSession GetSession(Guid sessionGuid);
    }
}
