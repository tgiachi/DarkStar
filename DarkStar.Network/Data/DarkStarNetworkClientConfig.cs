using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Network.Data
{
    public class DarkStarNetworkClientConfig
    {
        public string Address { get; set; } = "127.0.0.1";

        public int Port { get; set; } = 9000;
    }
}
