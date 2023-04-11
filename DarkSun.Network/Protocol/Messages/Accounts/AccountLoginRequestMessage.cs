using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Network.Attributes;
using DarkSun.Network.Protocol.Interfaces.Messages;
using DarkSun.Network.Protocol.Types;
using MessagePack;

namespace DarkSun.Network.Protocol.Messages.Accounts
{

    [NetworkMessage(DarkSunMessageType.AccountLoginRequest)]
    [MessagePackObject(keyAsPropertyName: true)]
    public class AccountLoginRequestMessage : IDarkSunNetworkMessage
    {
        public string Email { get; set; } = null!;
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
