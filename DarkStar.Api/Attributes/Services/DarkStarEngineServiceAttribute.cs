using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Api.Attributes.Services;

[AttributeUsage(AttributeTargets.Class)]
public class DarkStarEngineServiceAttribute : Attribute
{
    public string Name { get; set; }
    public int LoadOrder { get; set; } = 0;

    public DarkStarEngineServiceAttribute(string name, int loadOrder)
    {
        Name = name;
        LoadOrder = loadOrder;
    }
}
