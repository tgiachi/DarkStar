using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Attributes.Ai;
using DarkStar.Api.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace DarkStar.Engine.Utils
{
    public static class AiBehaviourRegistrationEx
    {
        public static IServiceCollection RegisterAiBehaviour(this IServiceCollection services)
        {
            foreach (var attrType in AssemblyUtils.GetAttribute<AiBehaviourAttribute>())
            {
                services.AddTransient(attrType);
            }

            return services;
        }
    }
}
