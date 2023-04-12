using System.Reflection;
using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Data.Config;
using DarkSun.Api.Engine.Data.ScriptEngine;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Api.Utils;
using DarkSun.Api.World.Types.Tiles;
using DarkSun.Engine.Attributes.ScriptEngine;
using DarkSun.Engine.Services.Base;
using Microsoft.Extensions.Logging;
using NLua;
using NLua.Exceptions;

namespace DarkSun.Engine.Services;

[DarkSunEngineService("ScriptEngine", 4)]
public class ScriptEngineService : BaseService<IScriptEngineService>, IScriptEngineService
{
    private readonly Lua _scriptEngine;
    private readonly DirectoriesConfig _directoriesConfig;
    private readonly IServiceProvider _container;

    public ScriptEngineService(ILogger<IScriptEngineService> logger, DirectoriesConfig directoriesConfig, IServiceProvider container) : base(logger)
    {
        _container = container;
        _directoriesConfig = directoriesConfig;
        _scriptEngine = new Lua() { UseTraceback = true };
    }

    protected override async ValueTask<bool> StartAsync()
    {
        await PrepareModuleDirectoryAsync();
        await PrepareScriptContextAsync();
        return true;
    }

    private ValueTask PrepareModuleDirectoryAsync()
    {
        var moduleFolder = _directoriesConfig[DirectoryNameType.ScriptModules];
        Logger.LogInformation("LUA script modules: {ScriptModules}", moduleFolder);

        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            moduleFolder = moduleFolder.Replace(@"\", @"\\");
        }

        _scriptEngine.DoString($@"
			-- Update the search path
			local module_folder = '{moduleFolder}'
			package.path = module_folder .. '?.lua;' .. package.path");

        return ValueTask.CompletedTask;
    }

    private async ValueTask PrepareScriptContextAsync()
    {
        Logger.LogInformation("Preparing Script Context");
        _scriptEngine["Engine"] = Engine;

        await ScanScriptModulesAsync();


        foreach (var tileType in Enum.GetValues<TileType>())
        {
            _scriptEngine[$"TILE_{tileType.ToString().ToUpper()}"] = (short)tileType;
        }

        var files = Directory.GetFiles(_directoriesConfig[DirectoryNameType.Scripts], "*.lua");
        Logger.LogInformation("Found {Count} scripts to load", files.Count());

        foreach (var file in files)
        {
            await ExecuteScriptAsync(file);
        }
    }

    private ValueTask ScanScriptModulesAsync()
    {
        foreach (var module in AssemblyUtils.GetAttribute<ScriptModuleAttribute>())
        {
            Logger.LogDebug("Found script module {Module}", module.Name);

            try
            {
                var instance = _container.GetService(module);

                foreach (var scriptMethod in module.GetMethods())
                {
                    var sMethodAttr = scriptMethod.GetCustomAttribute<ScriptFunctionAttribute>();

                    if (sMethodAttr == null)
                    {
                        continue;
                    }

                    Logger.LogInformation("Adding script method {M}", sMethodAttr.Alias ?? scriptMethod.Name);
                    _scriptEngine.RegisterFunction(sMethodAttr.Alias ?? scriptMethod.Name, instance!, scriptMethod);
                }

            }
            catch (Exception ex)
            {
                Logger.LogError("Error during initialize script module {Alias}: {Ex}", module.Name, ex);
            }


        }
        return ValueTask.CompletedTask;
    }

    private ValueTask ExecuteScriptAsync(string script)
    {
        try
        {
            Logger.LogInformation("Executing script: {Script}", new FileInfo(script).Name);
            _scriptEngine.DoFile(script);
        }
        catch (LuaScriptException ex)
        {
            Logger.LogError("Error during execute script {Script}: {Error}", script, ex);
        }

        return ValueTask.CompletedTask;
    }

    public override ValueTask<bool> StopAsync()
    {
        Logger.LogInformation("Disposing Script Engine");
        _scriptEngine.Dispose();

        return base.StopAsync();
    }

    public ScriptEngineExecutionResult ExecuteCommand(string command)
    {
        try
        {
            var result = new ScriptEngineExecutionResult { Result = _scriptEngine.DoString(command) };

            return result;
        }
        catch (Exception ex)
        {
            return new ScriptEngineExecutionResult() { Exception = ex };
        }
    }
}
