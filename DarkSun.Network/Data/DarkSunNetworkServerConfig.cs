using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DarkSun.Network.Data;

public class DarkSunNetworkServerConfig
{
    public string Address { get; set; } = IPAddress.Any.ToString();

    public int Port { get; set; } = 9000;
}
