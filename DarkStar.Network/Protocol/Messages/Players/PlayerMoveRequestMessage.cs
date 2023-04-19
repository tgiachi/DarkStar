using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Players
{

    [NetworkMessage(DarkStarMessageType.PlayerMoveRequest)]
    [ProtoContract]
    public struct PlayerMoveRequestMessage : IDarkStarNetworkMessage
    {
        [ProtoMember(1)]
        public PlayerMoveDirectionType Direction { get; set; }

        public PlayerMoveRequestMessage()
        {

        }

        public PlayerMoveRequestMessage(PlayerMoveDirectionType direction)
        {
            Direction = direction;
        }

    }
}
