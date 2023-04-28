using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Network.Protocol.Messages.Common;

namespace DarkStar.Api.Engine.Data.Sessions;

public class PlayerSession
{
    public Guid AccountId { get; set; }
    public string SessionId { get; set; }
    public Guid PlayerId { get; set; }
    public bool IsLogged { get; set; }
    public DateTime LastPingDateTime { get; set; }
    public string MapId { get; set; } = null!;
    public PointPosition Position { get; set; }
}
