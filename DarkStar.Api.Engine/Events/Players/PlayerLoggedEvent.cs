using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redbus.Events;

namespace DarkStar.Api.Engine.Events.Players
{
    public class PlayerLoggedEvent : EventBase
    {
        public Guid SessionId { get; set; }
        public Guid PlayerId { get; set; }
        public string MapId { get; set; } = null!;

        public PlayerLoggedEvent(Guid sessionId, Guid playerId, string mapId)
        {
            SessionId = sessionId;
            PlayerId = playerId;
            MapId = mapId;
        }

        public PlayerLoggedEvent()
        {
        }
    }
}
