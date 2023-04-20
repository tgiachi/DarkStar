using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Attributes.Commands;
using DarkStar.Api.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace DarkStar.Engine.Utils;

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
