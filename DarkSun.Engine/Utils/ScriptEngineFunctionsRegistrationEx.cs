using DarkSun.Api.Utils;
using DarkSun.Engine.Attributes.ScriptEngine;
using Microsoft.Extensions.DependencyInjection;

namespace DarkSun.Engine.Utils
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
