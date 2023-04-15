using DarkStar.Api.Utils;
using DarkStar.Engine.Attributes.ScriptEngine;
using Microsoft.Extensions.DependencyInjection;

namespace DarkStar.Engine.Utils
{
    public static class ScriptEngineFunctionsRegistrationEx
    {
        public static IServiceCollection RegisterScriptEngineFunctions(this IServiceCollection services)
        {
            foreach (var module in AssemblyUtils.GetAttribute<ScriptModuleAttribute>())
            {
                services.AddSingleton(module);
            }

            return services;
        }
    }
}
