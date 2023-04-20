using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Network.Data;

public class DarkStarNetworkServerConfig
{
    public string Address { get; set; } = IPAddress.Any.ToString();

    public int Port { get; set; } = 9000;
}
