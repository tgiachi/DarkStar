using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DarkSun.Network.Data
{
    public class DarkSunNetworkClientConfig
    {
        public string Address { get; set; } = "127.0.0.1";

        public int Port { get; set; } = 9000;
    }
}
