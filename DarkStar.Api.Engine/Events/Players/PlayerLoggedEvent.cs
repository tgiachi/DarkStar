using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Protocol.Messages.Common;
using Redbus.Events;

namespace DarkStar.Api.Engine.Events.Players;

public class PlayerLoggedEvent : EventBase
{
    public string SessionId { get; set; }
    public Guid PlayerId { get; set; }
    public string MapId { get; set; } = null!;

    public PointPosition Position { get; set; }

    public PlayerLoggedEvent(string sessionId, Guid playerId, string mapId, PointPosition position)
    {
        SessionId = sessionId;
        PlayerId = playerId;
        MapId = mapId;
        Position = position;
    }

    public PlayerLoggedEvent()
    {
    }
}
