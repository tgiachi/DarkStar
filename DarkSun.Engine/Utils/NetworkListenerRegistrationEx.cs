using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Attributes.Network;
using DarkSun.Api.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace DarkSun.Engine.Utils
{
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
}
