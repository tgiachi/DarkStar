using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Core;

namespace DarkStar.Api.Engine.Interfaces.Services.Base;

public interface IDarkSunEngineService : IAsyncDisposable
{
    ValueTask<bool> StartAsync(IDarkSunEngine engine);
    ValueTask<bool> StopAsync();
}
