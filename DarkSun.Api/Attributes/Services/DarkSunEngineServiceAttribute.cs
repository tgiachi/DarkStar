using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSun.Api.Attributes.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DarkSunEngineServiceAttribute : Attribute
    {
        public int LoadOrder { get; set; } = 0;
        public DarkSunEngineServiceAttribute(int loadOrder)
        {
            LoadOrder = loadOrder;
        }
    }
}
