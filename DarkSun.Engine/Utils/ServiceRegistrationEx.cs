using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace DarkSun.Engine.Utils
{
    public static class ServiceRegistrationEx
    {
        public static IServiceCollection RegisterDarkSunServices(this IServiceCollection services)
        {
            AssemblyUtils.GetAttribute<DarkSunEngineServiceAttribute>().ForEach(s =>
            {
                services.AddSingleton(AssemblyUtils.GetInterfaceOfType(s)!, s);
            });

            return services;
        }
    }
}
