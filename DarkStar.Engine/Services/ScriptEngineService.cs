using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Data.Config;
using DarkStar.Api.Engine.Data.ScriptEngine;
using DarkStar.Api.Engine.Events.Engine;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.Map.Enums;
using DarkStar.Api.Utils;
using DarkStar.Api.World.Types.Equippable;
using DarkStar.Api.World.Types.GameObjects;
using DarkStar.Api.World.Types.Items;
using DarkStar.Api.World.Types.Map;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Engine.Attributes.ScriptEngine;
using DarkStar.Engine.Services.Base;
using DarkStar.Network.Protocol.Messages.World;
using FastEnumUtility;
using GoRogue.GameFramework;
using Humanizer;
using Microsoft.Extensions.Logging;
using NLua;
using NLua.Exceptions;
using ProtoBuf;

namespace DarkStar.Engine.Services;

[DarkStarEngineService("ScriptEngine", 9)]
public class ScriptEngineService : BaseService<IScriptEngineService>, IScriptEngineService
{
    private readonly Lua _scriptEngine;
    private readonly DirectoriesConfig _directoriesConfig;
    private readonly IServiceProvider _container;
    private readonly ITypeService _typeService;

    public List<ScriptFunctionDescriptor> Functions { get; } = new();

    public Dictionary<string, object> ContextVariables { get; } = new();


    public ScriptEngineService(
        ILogger<ScriptEngineService> logger, DirectoriesConfig directoriesConfig, ITypeService typeService,
        IServiceProvider container
    ) : base(logger)
    {
        _typeService = typeService;
        _container = container;
        _directoriesConfig = directoriesConfig;
        _scriptEngine = new Lua { UseTraceback = true };
    }

    protected override async ValueTask<bool> StartAsync()
    {
        SubscribeToEvent<TileAddedEvent>(@event => AddTileToVariables(@event.Tile));
        SubscribeToEvent<GameObjectTypeAdded>(@event => AddGameObjectTypeToVariables(@event.GameObjectType));
        SubscribeToEvent<NpcTypeAdded>(@event => AddNpcTypeToVariables(@event.NpcType));
        SubscribeToEvent<NpcSubTypeAdded>(@event => AddNpcSubTypeToVariables(@event.NpcSubType));

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

        _scriptEngine.DoString(
            $@"
			-- Update the search path
			local module_folder = '{moduleFolder}'
			package.path = module_folder .. '?.lua;' .. package.path"
        );

        return ValueTask.CompletedTask;
    }

    private async ValueTask PrepareScriptContextAsync()
    {
        Logger.LogInformation("Preparing Script Context");
        _scriptEngine["ENGINE"] = Engine;


        await ScanScriptModulesAsync();


        foreach (var tileType in _typeService.Tiles)
        {
            AddTileToVariables(tileType);
        }

        foreach (var gameObject in _typeService.GameObjectTypes)
        {
            AddGameObjectTypeToVariables(gameObject);
        }

        foreach (var mapType in FastEnum.GetValues<MapType>())
        {
            var mapTypeName = $"MAP_TYPE_{mapType.ToString().ToUpper()}";
            Logger.LogDebug("Adding map type {MapTypeName}={Id} to LUA context", mapTypeName, (short)mapType);
            AddContextVariable(mapTypeName, (short)mapType);
        }

        foreach (var mapGeneratorType in FastEnum.GetValues<MapGeneratorType>())
        {
            var mapTypeName = $"MAP_GENERATOR_TYPE_{mapGeneratorType.ToString().Underscore().ToUpper()}";
            Logger.LogDebug("Adding map type {MapTypeName}={Id} to LUA context", mapTypeName, (short)mapGeneratorType);
            AddContextVariable(mapTypeName, (short)mapGeneratorType);
        }

        foreach (var messageTypeValue in FastEnum.GetValues<WorldMessageType>())
        {
            var messageType = "MESSAGE_TYPE_" + messageTypeValue.ToString().ToUpper();
            Logger.LogDebug("Adding message type {MessageType}={Id} to LUA context", messageType, (short)messageTypeValue);
            AddContextVariable(messageType, (short)messageTypeValue);
        }

        foreach (var equipLocation in FastEnum.GetValues<EquipLocationType>())
        {
            var equipLocationName = $"EQUIP_LOCATION_{equipLocation.ToString().ToUpper()}";
            Logger.LogDebug(
                "Adding equip location {EquipLocationName}={Id} to LUA context",
                equipLocationName,
                (short)equipLocation
            );
            AddContextVariable(equipLocationName, (short)equipLocation);
        }

        foreach (var itemRarity in FastEnum.GetValues<ItemRarityType>())
        {
            var itemRarityName = $"ITEM_RARITY_{itemRarity.ToString().ToUpper()}";
            Logger.LogDebug("Adding item rarity {ItemRarityName}={Id} to LUA context", itemRarityName, (short)itemRarity);
            AddContextVariable(itemRarityName, (short)itemRarity);
        }

        foreach (var npcType in _typeService.NpcTypes)
        {
            AddNpcTypeToVariables(npcType);
        }

        foreach (var npcSubType in _typeService.NpcSubTypes)
        {
            AddNpcSubTypeToVariables(npcSubType);
        }


        var files = Directory.GetFiles(_directoriesConfig[DirectoryNameType.Scripts], "*.lua");
        Logger.LogInformation("Found {Count} scripts to load", files.Count());

        foreach (var file in files)
        {
            await ExecuteScriptAsync(file);
        }
    }

    private void AddTileToVariables(Tile tileType)
    {
        var tileName = $"TILE_{tileType.FullName.ToUpper()}";
        Logger.LogDebug("Adding tile {TileName}={Id} to LUA context", tileName, tileType.Id);
        AddContextVariable(tileName, tileType.Id);
    }

    private void AddGameObjectTypeToVariables(GameObjectType gameObject)
    {
        var gameObjectName = $"GAMEOBJECT_{gameObject.Name.ToUpper()}";
        Logger.LogDebug("Adding game object {GameObjectName}={Id} to LUA context", gameObjectName, gameObject.Id);
        AddContextVariable(gameObjectName, gameObject.Id);
    }

    public void AddNpcTypeToVariables(NpcType npcType)
    {
        var npcTypeName = $"NPC_TYPE_{npcType.Name.ToUpper()}";
        Logger.LogDebug("Adding npc type {NpcTypeName}={Id} to LUA context", npcTypeName, npcType.Id);
        AddContextVariable(npcTypeName, npcType.Id);
    }

    private void AddNpcSubTypeToVariables(NpcSubType npcSubType)
    {
        var npcSubTypeName = $"NPC_SUBTYPE_{npcSubType.Name.ToUpper()}";
        Logger.LogDebug("Adding npc subtype {NpcSubTypeName}={Id} to LUA context", npcSubTypeName, npcSubType.Id);
        AddContextVariable(npcSubTypeName, npcSubType.Id);
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

                    ExtractFunctionDescriptor(sMethodAttr, scriptMethod);

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

    private void ExtractFunctionDescriptor(ScriptFunctionAttribute attribute, MethodInfo methodInfo)
    {
        var descriptor = new ScriptFunctionDescriptor
        {
            FunctionName = attribute.Alias ?? methodInfo.Name,
            Help = attribute.Help,
            Parameters = new List<ScriptFunctionParameterDescriptor>(),
            ReturnType = methodInfo.ReturnType.Name
        };

        foreach (var parameter in methodInfo.GetParameters())
        {
            descriptor.Parameters.Add(
                new ScriptFunctionParameterDescriptor
                {
                    ParameterName = parameter.Name,
                    ParameterType = parameter.ParameterType.Name
                }
            );
        }

        Functions.Add(descriptor);
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
