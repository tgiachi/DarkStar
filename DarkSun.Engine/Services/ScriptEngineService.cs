using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Engine.Services.Base;
using Microsoft.Extensions.Logging;
using NLua;

namespace DarkSun.Engine.Services
{
    [DarkSunEngineService("ScriptEngine", 4)]
    public class ScriptEngineService : BaseService<IScriptEngineService> , IScriptEngineService
    {
        private readonly Lua _scriptEngine;
        public ScriptEngineService(ILogger<IScriptEngineService> logger) : base(logger)
        {
            _scriptEngine = new Lua() { UseTraceback = true };
        }

    }
}
