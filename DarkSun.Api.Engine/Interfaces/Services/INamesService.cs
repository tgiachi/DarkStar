using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Interfaces.Services.Base;

namespace DarkSun.Api.Engine.Interfaces.Services
{
    public interface INamesService : IDarkSunEngineService
    {
        string RandomCityName { get; }
        string RandomAnimalName { get; }
    }
}
