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

    [NetworkMessage(DarkSunMessageType.AccountLoginRequest)]
    [ProtoContract]
    public class AccountLoginRequestMessage : IDarkSunNetworkMessage
    {
        [ProtoMember(1)]
        public string Email { get; set; } = null!;
        [ProtoMember(2)]
        public string Password { get; set; } = null!;

        public AccountLoginRequestMessage()
        {
        }

        public AccountLoginRequestMessage(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
