using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Network.Protocol.Messages.World
{
    public enum WorldMessageType : short
    {
        Whisper = 5,
        Normal = 10,
        Yell = 15,
    }
}
