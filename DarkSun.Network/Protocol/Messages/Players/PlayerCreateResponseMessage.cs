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
    [NetworkMessage(DarkSunMessageType.PlayerCreateResponse)]
    [ProtoContract]
    public class PlayerCreateResponseMessage : IDarkSunNetworkMessage
    {
        [ProtoMember(1)]
        public bool Success { get; set; }

        [ProtoMember(2)]
        public Guid PlayerId { get; set; }

        public PlayerCreateResponseMessage() { }

        public PlayerCreateResponseMessage(bool success, Guid playerId)
        {
            Success = success;
            PlayerId = playerId;
        }

    }
}
