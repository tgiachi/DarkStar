using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;

using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Accounts
{
    [NetworkMessage(DarkStarMessageType.AccountCreateResponse)]
    [ProtoContract]
    public class AccountCreateResponseMessage : IDarkSunNetworkMessage
    {
        [ProtoMember(1)]
        public bool Success { get; set; }
        [ProtoMember(2)]
        public string? Message { get; set; }
        public AccountCreateResponseMessage() { }

        public AccountCreateResponseMessage(bool success, string? message)
        {
            Success = success;
            Message = message;
        }
    }
}
