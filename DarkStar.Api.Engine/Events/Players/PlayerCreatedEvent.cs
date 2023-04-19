using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redbus.Events;

namespace DarkStar.Api.Engine.Events.Players
{
    public class PlayerCreatedEvent : EventBase
    {
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; } = null!;

        public PlayerCreatedEvent(Guid playerId, string playerName)
        {
            PlayerId = playerId;
            PlayerName = playerName;
        }


        public PlayerCreatedEvent()
        {

        }

    }
}
