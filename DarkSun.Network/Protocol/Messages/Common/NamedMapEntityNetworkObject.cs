using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace DarkStar.Network.Protocol.Messages.Common
{

    [ProtoContract]
    public class NamedMapEntityNetworkObject : MapEntityNetworkObject
    {
        [ProtoMember(5)]
        public string Name { get; set; } = null!;
    }

}
