using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSun.Api.Engine.Interfaces.Services.Base
{
    public interface IDarkSunEngineService : IAsyncDisposable
    {
        ValueTask<bool> StartAsync();
        ValueTask<bool> StopAsync();
    }
}
