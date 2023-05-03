using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redbus.Events;


namespace DarkStar.Api.Engine.Events.Players;

public class PlayerGoldChangedEvent : EventBase
{
    public Guid PlayerId { get; set; }
    public int Gold { get; set; }
    public int TotalGold { get; set; }

    public PlayerGoldChangedEvent(Guid playerId, int gold, int totalGold)
    {
        PlayerId = playerId;
        Gold = gold;
        TotalGold = totalGold;
    }

    public PlayerGoldChangedEvent()
    {
    }
}
