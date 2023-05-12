using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Data.Config;
using DarkStar.Api.Engine.Data.Config;
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
using DarkStar.Engine.CodeGenerator;
using DarkStar.Engine.Services.Base;
using DarkStar.Network.Protocol.Messages.Common;
using DarkStar.Network.Protocol.Messages.World;
using Esprima;
using FastEnumUtility;
using Humanizer;
using Jint;
using Jint.Runtime;
using Microsoft.Extensions.Logging;


namespace DarkStar.Engine.Services;

[DarkStarEngineService("ScriptEngine", 9)]
public class ScriptEngineService : BaseService<IScriptEngineService>, IScriptEngineService
{
    private readonly Jint.Engine _scriptEngine;
    private readonly DirectoriesConfig _directoriesConfig;
    private readonly IServiceProvider _container;
    private readonly ITypeService _typeService;

    private readonly Dictionary<string, object> _scriptConstants = new();

    private readonly string _fileExtension = "js";

    public List<ScriptFunctionDescriptor> Functions { get; } = new();

    public Dictionary<string, object> ContextVariables { get; } = new();


    public ScriptEngineService(
        ILogger<ScriptEngineService> logger, DirectoriesConfig directoriesConfig, ITypeService typeService,
        EngineConfig engineConfig,
        IServiceProvider container
    ) : base(logger)
    {
        _typeService = typeService;
        _container = container;
        _directoriesConfig = directoriesConfig;
        _scriptEngine = new Jint.Engine(
            options =>
            {
                options.DebugMode(engineConfig.Logger.EnableDebug);
                //options.TimeoutInterval(TimeSpan.FromSeconds(4));
                // Limit the memory to 4Gb
                options.LimitMemory(4_000_000_000);
                options.AllowClr(AssemblyUtils.GetAppAssemblies().ToArray());


                options.EnableModules(_directoriesConfig[DirectoryNameType.ScriptModules]);
                options.StringCompilationAllowed = true;
            }
        );
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
        Logger.LogInformation("JS script modules: {ScriptModules}", moduleFolder);


        return ValueTask.CompletedTask;
    }

    private async ValueTask PrepareScriptContextAsync()
    {
        Logger.LogInformation("Preparing Script Context");
        _scriptEngine.SetValue("Engine", Engine);


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
            Logger.LogDebug("Adding map type {MapTypeName}={Id} to JS context", mapTypeName, (short)mapType);
            AddContextVariable(mapTypeName, (short)mapType);
        }

        foreach (var mapGeneratorType in FastEnum.GetValues<MapGeneratorType>())
        {
            var mapTypeName = $"MAP_GENERATOR_TYPE_{mapGeneratorType.ToString().Underscore().ToUpper()}";
            Logger.LogDebug("Adding map type {MapTypeName}={Id} to JS context", mapTypeName, (short)mapGeneratorType);
            AddContextVariable(mapTypeName, (short)mapGeneratorType);
        }

        foreach (var messageTypeValue in FastEnum.GetValues<WorldMessageType>())
        {
            var messageType = "MESSAGE_TYPE_" + messageTypeValue.ToString().ToUpper();
            Logger.LogDebug("Adding message type {MessageType}={Id} to JS context", messageType, (short)messageTypeValue);
            AddContextVariable(messageType, (short)messageTypeValue);
        }

        foreach (var equipLocation in FastEnum.GetValues<EquipLocationType>())
        {
            var equipLocationName = $"EQUIP_LOCATION_{equipLocation.ToString().ToUpper()}";
            Logger.LogDebug(
                "Adding equip location {EquipLocationName}={Id} to JS context",
                equipLocationName,
                (short)equipLocation
            );
            AddContextVariable(equipLocationName, (short)equipLocation);
        }

        foreach (var itemRarity in FastEnum.GetValues<ItemRarityType>())
        {
            var itemRarityName = $"ITEM_RARITY_{itemRarity.ToString().ToUpper()}";
            Logger.LogDebug("Adding item rarity {ItemRarityName}={Id} to JS context", itemRarityName, (short)itemRarity);
            AddContextVariable(itemRarityName, (short)itemRarity);
        }

        foreach (var direction in FastEnum.GetValues<MoveDirectionType>())
        {
            var directionName = "MOVE_DIRECTION_" + direction.ToString().Underscore().ToUpper();
            Logger.LogDebug("Adding direction {DirectionName}={Id} to JS context", directionName, (short)direction);
            AddContextVariable(directionName, (short)direction);
        }

        foreach (var npcType in _typeService.NpcTypes)
        {
            AddNpcTypeToVariables(npcType);
        }

        foreach (var npcSubType in _typeService.NpcSubTypes)
        {
            AddNpcSubTypeToVariables(npcSubType);
        }


        var files = Directory.GetFiles(_directoriesConfig[DirectoryNameType.Scripts], "*." + _fileExtension);
        Logger.LogInformation("Found {Count} scripts to load", files.Count());

        foreach (var file in files)
        {
            await ExecuteScriptAsync(file);
        }
    }

    private void AddTileToVariables(Tile tileType)
    {
        var tileName = $"TILE_{tileType.FullName.ToUpper()}";
        Logger.LogDebug("Adding tile {TileName}={Id} to JS context", tileName, tileType.Id);
        AddContextVariable(tileName, tileType.Id);
    }

    private void AddGameObjectTypeToVariables(GameObjectType gameObject)
    {
        var gameObjectName = $"GAMEOBJECT_{gameObject.Name.ToUpper()}";
        Logger.LogDebug("Adding game object {GameObjectName}={Id} to JS context", gameObjectName, gameObject.Id);
        AddContextVariable(gameObjectName, gameObject.Id);
    }

    public void AddNpcTypeToVariables(NpcType npcType)
    {
        var npcTypeName = $"NPC_TYPE_{npcType.Name.ToUpper()}";
        Logger.LogDebug("Adding npc type {NpcTypeName}={Id} to JS context", npcTypeName, npcType.Id);
        AddContextVariable(npcTypeName, npcType.Id);
    }

    private void AddNpcSubTypeToVariables(NpcSubType npcSubType)
    {
        var npcSubTypeName = $"NPC_SUBTYPE_{npcSubType.Name.ToUpper()}";
        Logger.LogDebug("Adding npc subtype {NpcSubTypeName}={Id} to JS context", npcSubTypeName, npcSubType.Id);
        AddContextVariable(npcSubTypeName, npcSubType.Id);
    }

    private void AddContextVariable(string name, object value)
    {
        _scriptEngine.SetValue(name, value);
        ContextVariables[name] = value;
    }

    private ValueTask ScanScriptModulesAsync()
    {
        foreach (var module in AssemblyUtils.GetAttribute<ScriptModuleAttribute>())
        {
            Logger.LogDebug("Found script module {Module}", module.Name);

            try
            {
                var obj = _container.GetService(module);

                foreach (var scriptMethod in module.GetMethods())
                {
                    var sMethodAttr = scriptMethod.GetCustomAttribute<ScriptFunctionAttribute>();

                    if (sMethodAttr == null)
                    {
                        continue;
                    }

                    ExtractFunctionDescriptor(sMethodAttr, scriptMethod);

                    Logger.LogInformation("Adding script method {M}", sMethodAttr.Alias ?? scriptMethod.Name);

                    _scriptEngine.SetValue(
                        sMethodAttr.Alias ?? scriptMethod.Name,
                        CreateJsEngineDelegate(obj, scriptMethod)
                    );
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error during initialize script module {Alias}: {Ex}", module.Name, ex);
            }
        }

        return ValueTask.CompletedTask;
    }

    private Delegate CreateJsEngineDelegate(object obj, MethodInfo method)
    {
        return method.CreateDelegate(
            Expression.GetDelegateType(
                (from parameter in method.GetParameters() select parameter.ParameterType)
                .Concat(new[] { method.ReturnType })
                .ToArray()
            ),
            obj
        );
    }

    private void ExtractFunctionDescriptor(ScriptFunctionAttribute attribute, MethodInfo methodInfo)
    {
        var descriptor = new ScriptFunctionDescriptor
        {
            FunctionName = attribute.Alias ?? methodInfo.Name,
            Help = attribute.Help,
            Parameters = new List<ScriptFunctionParameterDescriptor>(),
            ReturnType = methodInfo.ReturnType.Name,
            RawReturnType = methodInfo.ReturnType
        };

        foreach (var parameter in methodInfo.GetParameters())
        {
            descriptor.Parameters.Add(
                new ScriptFunctionParameterDescriptor
                {
                    ParameterName = parameter.Name,
                    ParameterType = parameter.ParameterType.Name,
                    RawParameterType = parameter.ParameterType
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
            _scriptEngine.Execute(File.ReadAllText(script), ParserOptions.Default);
        }
        catch (JintException ex)
        {
            Logger.LogError("Error during execute script {Script}: {Error}", script, ex);
            throw;
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
            var result = new ScriptEngineExecutionResult { Result = _scriptEngine.Evaluate(command) };

            return result;
        }
        catch (Exception ex)
        {
            return new ScriptEngineExecutionResult() { Exception = ex };
        }
    }

    public async Task<string> GenerateTypeDefinitionsAsync()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"// This file is generated by the server on {DateTime.Now} . Do not edit it manually.");
        sb.AppendLine("");
        sb.AppendLine("// Declare enums");
        sb.AppendLine(TypeScriptCodeGenerator.GenerateTypeDefinitionOfEnum<MoveDirectionType>());
        sb.AppendLine(TypeScriptCodeGenerator.GenerateTypeDefinitionOfEnum<ItemRarityType>());
        sb.AppendLine(TypeScriptCodeGenerator.GenerateTypeDefinitionOfEnum<MapType>());
        sb.AppendLine(TypeScriptCodeGenerator.GenerateTypeDefinitionOfEnum<WorldMessageType>());
        sb.AppendLine(TypeScriptCodeGenerator.GenerateTypeDefinitionOfEnum<EquipLocationType>());
        sb.AppendLine(TypeScriptCodeGenerator.GenerateTypeDefinitionOfEnum<MapGeneratorType>());

        foreach (var interf in Functions.SelectMany(f => f.Parameters)
                     .Select(p => p.RawParameterType)
                     .Where(t => TypeScriptCodeGenerator.GetTypeScriptType(t) == "any"))
        {
            sb.AppendLine(TypeScriptCodeGenerator.GenerateInterface(interf));
        }

        sb.AppendLine(TypeScriptCodeGenerator.GenerateTypeDefinitionOfConstants(ContextVariables));
        sb.AppendLine("");
        sb.AppendLine("// Declare functions");

        foreach (var function in Functions)
        {
            sb.AppendLine(TypeScriptCodeGenerator.GenerateTypeDefinitionOfFunction(function));
        }

        return sb.ToString();
    }
}
