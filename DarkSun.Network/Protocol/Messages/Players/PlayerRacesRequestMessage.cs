using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Attributes;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;
using ProtoBuf;


namespace DarkSun.Network.Protocol.Messages.Players
{

    [ProtoContract]
    [NetworkMessage(DarkSunMessageType.PlayerRacesRequest)]
    public class PlayerRacesRequestMessage : IDarkSunNetworkMessage
    {
    }
}
