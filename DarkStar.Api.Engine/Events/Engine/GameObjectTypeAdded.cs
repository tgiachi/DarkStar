using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.GameObjects;
using Redbus.Events;

namespace DarkStar.Api.Engine.Events.Engine;

public class GameObjectTypeAdded : EventBase
{
    public GameObjectType GameObjectType { get; set; }
}
