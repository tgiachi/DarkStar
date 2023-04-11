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

                var interf = AssemblyUtils.GetInterfacesOfType(s)!.First(k => k.Name.EndsWith(s.Name));
                services.AddSingleton(interf, s);
            });

            return services;
        }
    }
}
