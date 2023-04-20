using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Data.Config.Sections;

namespace DarkStar.Api.Engine.Data.Config;

public class EngineConfig
{
    public DatabaseConfig Database { get; set; } = new();
    public NetworkServerConfig NetworkServer { get; set; } = new();
    public LoggerConfig Logger { get; set; } = new();
    public AssemblyConfig Assemblies { get; set; } = new();
    public HttpServerConfig HttpServer { get; set; } = new();
    public MapConfig Maps { get; set; } = new();
}
