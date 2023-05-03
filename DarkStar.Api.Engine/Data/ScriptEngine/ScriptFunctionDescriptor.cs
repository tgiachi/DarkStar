using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Api.Engine.Data.ScriptEngine;

public class ScriptFunctionDescriptor
{
    public string FunctionName { get; set; } = null!;
    public string? Help { get; set; }

    public List<ScriptFunctionParameterDescriptor> Parameters { get; set; } = new();
    public string ReturnType { get; set; }
}

public class ScriptFunctionParameterDescriptor
{
    public string ParameterName { get; set; } = null!;
    public string ParameterType { get; set; } = null!;
}
