using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Core;
using DarkStar.Api.Engine.ScriptModules;
using DarkStar.Engine.Attributes.ScriptEngine;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.ScriptModules;

[ScriptModule]
public class ServerScriptModule : BaseScriptModule
{
    public ServerScriptModule(ILogger<BaseScriptModule> logger, IDarkSunEngine engine) : base(logger, engine)
    {
    }

    [ScriptFunction("set_server_motd")]
    public void SetServerMotd(string serverMotd)
    {
        Engine.ServerMotd = serverMotd;
        Logger.LogInformation("Motd set");
    }

    [ScriptFunction("set_server_name")]
    public void SetServerName(string serverName)
    {
        Engine.ServerName = serverName;
        Logger.LogInformation("Server name set to {Name}", serverName);
    }
}
