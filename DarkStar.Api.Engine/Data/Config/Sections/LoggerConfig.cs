using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Api.Engine.Data.Config.Sections;

public class LoggerConfig
{
    public bool EnableDebug { get; set; } = false;

    public bool EnableFileLogging { get; set; } = true;
}
