using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSun.Engine.Interfaces.Services.Base
{
    public interface IDarkSunEngineService : IAsyncDisposable
    {
        string Name { get; }
        int LoadOrder { get; }
        Task<bool> StartAsync();
        Task<bool> StopAsync();
    }
}
