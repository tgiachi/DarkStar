using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSun.Engine.Interfaces.Services
{
    public interface IDarkSunEngineService
    {
        string Name { get; }
        int LoadOrder { get; }
        Task<bool> Start();
        Task<bool> Stop();
    }
}
