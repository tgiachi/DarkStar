using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Interfaces.Services.Base;

namespace DarkSun.Api.Engine.Interfaces.Services
{
    public interface IJobSchedulerService : IDarkSunEngineService
    {
        void AddJob(string name, Action action, int seconds, bool runOnce);
        void RemoveJob(string name);
    }
}
