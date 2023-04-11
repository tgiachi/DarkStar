using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Interfaces.Core;
using DarkSun.Api.Engine.Interfaces.Services.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.Runner
{
    public class DarkEngineHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IServiceProvider _serviceProvider;
        public DarkEngineHostedService(ILogger<DarkEngineHostedService> logger,
            IHostApplicationLifetime lifetime,
            IServiceProvider serviceProvider)
        {
            _applicationLifetime = lifetime;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _serviceProvider.GetRequiredService<IDarkSunEngine>().StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _serviceProvider.GetRequiredService<IDarkSunEngine>().StopAsync();
        }
    }
}
