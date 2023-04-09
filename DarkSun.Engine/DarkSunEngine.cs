using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Data.Config;
using DarkSun.Engine.Interfaces.Core;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine
{
    public class DarkSunEngine : IDarkSunEngine
    {
        private readonly ILogger _logger;
        private readonly DirectoriesConfig _directoriesConfig;


        public DarkSunEngine(ILogger<DarkSunEngine> logger, DirectoriesConfig directoriesConfig)
        {
            _logger = logger;
            _directoriesConfig = directoriesConfig;
        }
    }
}
