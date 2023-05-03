using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Data.ScriptEngine;
using DarkStar.Api.Engine.Interfaces.Services.Base;

namespace DarkStar.Api.Engine.Interfaces.Services;

public interface IScriptEngineService : IDarkSunEngineService
{
    ScriptEngineExecutionResult ExecuteCommand(string command);

    List<ScriptFunctionDescriptor> Functions { get; }

    Dictionary<string, object> ContextVariables { get; }
}
