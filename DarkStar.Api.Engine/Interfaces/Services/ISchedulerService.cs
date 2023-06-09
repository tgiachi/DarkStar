using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Services.Base;

namespace DarkStar.Api.Engine.Interfaces.Services;

public interface ISchedulerService : IDarkSunEngineService
{
    delegate Task OnTickDelegate(double deltaTime);

    event OnTickDelegate? OnTick;
}
