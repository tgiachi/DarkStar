using System.Reflection;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Data.Config;
using DarkStar.Api.Engine.Data.ScriptEngine;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Utils;
using DarkStar.Api.World.Types.Map;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Engine.Attributes.ScriptEngine;
using DarkStar.Engine.Services.Base;
using FastEnumUtility;
using GoRogue.GameFramework;
using Microsoft.Extensions.Logging;
using NLua;
using NLua.Exceptions;

namespace DarkStar.Engine.Services;

[DarkStarEngineService("ScriptEngine", 9)]
public class ScriptEngineService : BaseService<IScriptEngineService>, IScriptEngineService
{
    private readonly Lua _scriptEngine;
    private readonly DirectoriesConfig _directoriesConfig;
    private readonly IServiceProvider _container;
    private readonly ITypeService _typeService;

    public Dictionary<string, object> ContextVariables { get; } = new(); 



    public ScriptEngineService(ILogger<IScriptEngineService> logger, DirectoriesConfig directoriesConfig, ITypeService typeService,
        IServiceProvider container) : base(logger)
    {
        _typeService = typeService;
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


        foreach (var tileType in _typeService.Tiles)
        {
            var tileName = $"TILE_{tileType.FullName.ToUpper()}";
            Logger.LogDebug("Adding tile {TileName}={Id} to LUA context", tileName, tileType.Id);
            AddContextVariable(tileName, tileType.Id);
        }

        foreach (var gameObject in _typeService.GameObjectTypes)
        {
            var gameObjectName = $"GAMEOBJECT_{gameObject.Name.ToUpper()}";
            Logger.LogDebug("Adding game object {GameObjectName}={Id} to LUA context", gameObjectName, gameObject.Id);
            AddContextVariable(gameObjectName, gameObject.Id);

        }

        foreach (var mapType in FastEnum.GetValues<MapType>())
        {
            var mapTypeName = $"MAP_TYPE_{mapType.ToString().ToUpper()}";
            Logger.LogDebug("Adding map type {MapTypeName}={Id} to LUA context", mapTypeName, (short)mapType);
            AddContextVariable(mapTypeName, (short)mapType);
           
        }


        var files = Directory.GetFiles(_directoriesConfig[DirectoryNameType.Scripts], "*.lua");
        Logger.LogInformation("Found {Count} scripts to load", files.Count());

        foreach (var file in files)
        {
            await ExecuteScriptAsync(file);
        }
    }

    private void AddContextVariable(string name, object value)
    {
        _scriptEngine[name] = value;
        ContextVariables[name] = value;
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

  

    public void AddVariable(string name, object value)
    {
        _scriptEngine[name] = value;
    }
}
