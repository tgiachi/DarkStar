using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Attributes.Commands;
using DarkSun.Api.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace DarkSun.Engine.Utils
{
    public static class CommandsRegistrationEx
    {
        public static IServiceCollection RegisterCommandExecutors(this IServiceCollection services)
        {
            foreach (var commandExecutorType in AssemblyUtils.GetAttribute<CommandActionAttribute>())
            {
                services.AddSingleton(commandExecutorType);
            }

            return services;
        }
    }
}
