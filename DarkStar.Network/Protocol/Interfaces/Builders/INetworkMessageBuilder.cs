using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Data;
using DarkStar.Network.Protocol.Interfaces.Messages;

namespace DarkStar.Network.Protocol.Interfaces.Builders;

public interface INetworkMessageBuilder
{
    byte[] GetMessageSeparators { get; }
    NetworkMessageData ParseMessage(byte[] buffer);

    byte[] BuildMessage<T>(T message) where T : IDarkSunNetworkMessage;

    int GetMessageLength(byte[] buffer);
}
