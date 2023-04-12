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

    [NetworkMessage(DarkSunMessageType.AccountLoginResponse)]
    [ProtoContract]
    public class AccountLoginResponseMessage : IDarkSunNetworkMessage
    {
        [ProtoMember(1)]
        public bool Success { get; set; }

        public AccountLoginResponseMessage()
        {
        }

        public AccountLoginResponseMessage(bool success)
        {
            Success = success;
        }
    }
}
