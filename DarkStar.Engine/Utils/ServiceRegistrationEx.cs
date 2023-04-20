using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace DarkStar.Engine.Utils;

public static class ServiceRegistrationEx
{
    public static IServiceCollection RegisterDarkSunServices(this IServiceCollection services)
    {
        AssemblyUtils.GetAttribute<DarkStarEngineServiceAttribute>().ForEach(s =>
        {
            var interf = AssemblyUtils.GetInterfacesOfType(s)!.First(k => k.Name.EndsWith(s.Name));
            services.AddSingleton(interf, s);
        });

        return services;
    }
}
