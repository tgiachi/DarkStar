using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Engine.Attributes.ScriptEngine;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ScriptFunctionAttribute : Attribute
{
    public string Alias { get; set; } = null!;

    public ScriptFunctionAttribute(string? alias)
    {
        Alias = alias ?? string.Empty;
    }
}
