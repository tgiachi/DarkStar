using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Players
{

    [NetworkMessage(DarkStarMessageType.PlayerLoginRequest)]
    [ProtoContract]
    public struct PlayerLoginRequestMessage : IDarkStarNetworkMessage
    {
        [ProtoMember(1)]
        public Guid PlayerId { get; set; }

        public PlayerLoginRequestMessage(Guid playerId)
        {
            PlayerId = playerId;
        }

        public PlayerLoginRequestMessage()
        {

        }
    }
  
}
