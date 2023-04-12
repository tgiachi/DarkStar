using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Attributes;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;
using ProtoBuf;

namespace DarkSun.Network.Protocol.Messages.Accounts
{
    [NetworkMessage(DarkSunMessageType.AccountCreateResponse)]
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
