using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Core;
using Microsoft.Extensions.Logging;

namespace DarkStar.Api.Engine.ScriptModules
{
    public class BaseScriptModule
    {
        protected ILogger Logger { get; }
        protected IDarkSunEngine Engine { get; }
        public BaseScriptModule(ILogger<BaseScriptModule> logger, IDarkSunEngine engine)
        {
            Logger = logger;
            Engine = engine;
        }
    }
}
