using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Interfaces.Core;

namespace DarkSun.Api.Engine.Interfaces.Services.Base;

public interface IDarkSunEngineService : IAsyncDisposable
{
    ValueTask<bool> StartAsync(IDarkSunEngine engine);
    ValueTask<bool> StopAsync();
}
