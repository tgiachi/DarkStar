using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Attributes.Network;
using DarkStar.Api.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace DarkStar.Engine.Utils;

public static class NetworkListenerRegistrationEx
{
    public static IServiceCollection RegisterMessageListeners(this IServiceCollection services)
    {
        foreach (var service in AssemblyUtils.GetAttribute<NetworkMessageListenerAttribute>())
        {
            services.AddSingleton(service);
        }

        foreach (var service in AssemblyUtils.GetAttribute<NetworkConnectionHandlerAttribute>())
        {
            services.AddSingleton(service);
        }

        return services;
    }
}
