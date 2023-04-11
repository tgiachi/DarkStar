using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Data.Config;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Api.World.Types.Tiles;
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

    public ScriptEngineService(ILogger<IScriptEngineService> logger, DirectoriesConfig directoriesConfig) : base(logger)
    {
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
}
