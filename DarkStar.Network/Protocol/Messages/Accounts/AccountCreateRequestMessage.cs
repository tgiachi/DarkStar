using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Attributes;
using DarkStar.Network.Protocol.Interfaces.Messages;
using DarkStar.Network.Protocol.Types;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Accounts;

[NetworkMessage(DarkStarMessageType.AccountCreateRequest)]
[ProtoContract]
public class AccountCreateRequestMessage : IDarkStarNetworkMessage
{
    [ProtoMember(1)] public string Email { get; set; }
    [ProtoMember(2)] public string Password { get; set; }

    public AccountCreateRequestMessage(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public AccountCreateRequestMessage()
    {
    }
}
