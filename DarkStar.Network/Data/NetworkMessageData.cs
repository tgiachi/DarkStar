using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;


namespace DarkStar.Network.Data;

public class NetworkMessageData
{
    public DarkStarMessageType MessageType { get; set; }

    public IDarkStarNetworkMessage Message { get; set; } = null!;
}
