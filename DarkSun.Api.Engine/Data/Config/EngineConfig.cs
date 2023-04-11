using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Data.Config.Sections;

namespace DarkSun.Api.Engine.Data.Config;

public class EngineConfig
{
    public DatabaseConfig Database { get; set; } = new();
    public NetworkServerConfig NetworkServer { get; set; } = new();
    public LoggerConfig Logger { get; set; } = new();
}
