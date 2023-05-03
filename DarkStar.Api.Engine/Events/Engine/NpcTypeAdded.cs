using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Npc;
using Redbus.Events;

namespace DarkStar.Api.Engine.Events.Engine;

public class NpcTypeAdded : EventBase
{
    public NpcType NpcType { get; set; }
}
