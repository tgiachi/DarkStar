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
    [ProtoContract]
    [NetworkMessage(DarkStarMessageType.PlayerMoveResponse)]
    public struct PlayerMoveResponseMessage : IDarkStarNetworkMessage
    {
        [ProtoMember(1)]
        public PointPosition Position { get; set; }


        public PlayerMoveResponseMessage(PointPosition position) => Position = position;

        public PlayerMoveResponseMessage()
        {

        }
    }
}
