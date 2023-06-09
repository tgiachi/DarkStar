using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;

namespace DarkStar.Network.Interfaces;

public interface INetworkClientMessageListener
{
    Task OnMessageReceivedAsync(DarkStarMessageType messageType, IDarkStarNetworkMessage message);
}
