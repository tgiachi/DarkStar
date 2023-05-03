using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Attributes.Objects;
using DarkStar.Api.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace DarkStar.Engine.Utils;

public static class WorldObjectAndItemsRegistrationEx
{
    public static IServiceCollection RegisterWorldObjectAndItems(this IServiceCollection services)
    {
        foreach (var attr in AssemblyUtils.GetAttribute<GameObjectActionAttribute>())
        {
            services.AddTransient(attr);
        }

        return services;
    }
}
