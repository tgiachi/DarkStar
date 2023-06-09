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

[NetworkMessage(DarkStarMessageType.AccountLoginResponse)]
[ProtoContract]
public struct AccountLoginResponseMessage : IDarkStarNetworkMessage
{
    [ProtoMember(1)] public bool Success { get; set; }

    public AccountLoginResponseMessage()
    {
    }

    public AccountLoginResponseMessage(bool success) => Success = success;
}
