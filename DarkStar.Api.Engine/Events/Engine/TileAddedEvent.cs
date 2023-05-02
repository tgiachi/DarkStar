using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.World.Types.Tiles;
using Redbus.Events;

namespace DarkStar.Api.Engine.Events.Engine;
public class TileAddedEvent : EventBase
{
    public Tile Tile { get; set; }
}