using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Npc;
using Redbus.Events;

namespace DarkStar.Api.Engine.Events.Engine;
public class NpcSubTypeAdded : EventBase
{
    public NpcSubType NpcSubType { get; set; }
}
