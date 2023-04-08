using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;

namespace DarkSun.Network.Interfaces
{
    public interface INetworkMessageListener
    {
        Task OnMessageReceived(Guid sessionId, DarkSunMessageType messageType, IDarkSunNetworkMessage message);
    }
}
