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
    public string? Help { get; set; }

    public ScriptFunctionAttribute(string? alias, string? help = null)
    {
        Alias = alias ?? string.Empty;
        Help = help;
    }
}
