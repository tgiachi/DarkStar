using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Data;
using DarkSun.Network.Protocol.Interfaces.Messages;

namespace DarkSun.Network.Protocol.Interfaces.Builders
{
    public interface INetworkMessageBuilder
    {
        NetworkMessageData ParseMessage(byte[] buffer);

        byte[] BuildMessage<T>(T message) where T : IDarkSunNetworkMessage;

        int GetMessageLength(byte[] buffer);
    }
}
