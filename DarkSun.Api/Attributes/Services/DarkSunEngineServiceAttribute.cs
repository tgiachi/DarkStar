using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSun.Api.Attributes.Services;

[AttributeUsage(AttributeTargets.Class)]
public class DarkSunEngineServiceAttribute : Attribute
{
    public string Name { get; set; }
    public int LoadOrder { get; set; } = 0;

    public DarkSunEngineServiceAttribute(string name, int loadOrder)
    {
        Name = name;
        LoadOrder = loadOrder;
    }
}
