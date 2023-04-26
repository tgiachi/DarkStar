using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Data.Config;
using DarkStar.Api.Engine.Data.Templates;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Engine.Map.Entities;
using DarkStar.Api.Engine.Utils;
using DarkStar.Api.Utils;
using DarkStar.Api.World.Types.GameObjects;
using DarkStar.Api.World.Types.Map;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Database.Entities.Npc;
using DarkStar.Database.Entities.Objects;
using DarkStar.Engine.Services.Base;
using DarkStar.Network.Protocol.Messages.Common;
using FastEnumUtility;
using Microsoft.Extensions.Logging;
using NetTopologySuite.GeometriesGraph;
using TiledSharp;

namespace DarkStar.Engine.Services;

[DarkStarEngineService(nameof(BlueprintService), 2)]
public class BlueprintService : BaseService<BlueprintService>, IBlueprintService
{
    private readonly DirectoriesConfig _directoriesConfig;
    private readonly INamesService _namesService;
    private readonly List<BluePrintTemplate> _bluePrintTemplates = new();

    public BlueprintService(ILogger<BlueprintService> logger, DirectoriesConfig directoriesConfig, INamesService namesService) : base(logger)
    {
        _directoriesConfig = directoriesConfig;
        _namesService = namesService;

    }

    protected override async ValueTask<bool> StartAsync()
    {
        await ScanForMapTemplatesAsync();

        return true;
    }

    private async ValueTask ScanForMapTemplatesAsync()
    {
        var mapTemplates = Directory.GetFiles(_directoriesConfig[DirectoryNameType.BluePrints], "*.tmx", SearchOption.AllDirectories);

        foreach (var mapTemplate in mapTemplates)
        {
            await LoadMapTemplateAsync(mapTemplate);
        }
    }



    private ValueTask LoadMapTemplateAsync(string fileName)
    {
        try
        {
            var template = new TmxMap(fileName);

            // Check if the map is valid
            if (template.Layers.Count != FastEnum.GetValues<MapLayer>().Count)
            {
                Logger.LogWarning("Map have {MapCount} layers, template must have {MapLayerCount} ", template.ImageLayers.Count, FastEnum.GetValues<MapLayer>().Count);
                return ValueTask.CompletedTask;
            }

            foreach (var templateDefinition in template.ObjectGroups)
            {
                foreach (var templateObject in templateDefinition.Objects)
                {
                    Logger.LogInformation("Adding [{Class}] {Name} ", templateObject.Type, templateObject.Name);
                    var points = GetPointsFromRect(templateObject.X, templateObject.Y, templateObject.Width, templateObject.Height, template.Tilesets.First());
                    _bluePrintTemplates.Add(BuildBluePrintTemplateFromObject(template, templateObject.Name, templateObject.Type, points));
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load map template {FileName}: {Error}", fileName, ex);
        }
        return ValueTask.CompletedTask;
    }

    private List<PointPosition> GetPointsFromRect(double x, double y, double width, double height, TmxTileset tileSet)
    {
        var points = new List<PointPosition>();
        for (var i = x * tileSet.TileWidth; i < width / tileSet.TileWidth; i++)
        {
            for (var j = y * tileSet.TileHeight; j < height / tileSet.TileHeight; j++)
            {
                points.Add(new PointPosition((int)i, (int)j));
            }
        }
        return points;
    }

    private BluePrintTemplate BuildBluePrintTemplateFromObject(
        TmxMap map, string objectName, string className, List<PointPosition> points)
    {
        var bluePrintTemplate = new BluePrintTemplate
        {
            Name = objectName,
            ClassName = className,
        };

        foreach (var layer in map.Layers.Reverse())
        {
            var layerType = FastEnum.Parse<MapLayer>(layer.Name);
            foreach (var p in points)
            {
                var tile = layer.Tiles.FirstOrDefault(s => s.X == p.X && s.Y == p.Y);
                if (tile != null)
                {
                    try
                    {
                      
                            bluePrintTemplate.Layers[layerType].Add(new BluePrintTemplatePoint(tile.Gid, p.X, p.Y));
                       
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Failed to parse tile type {TileType}", tile.Gid);
                    }
                }
            }
        }

        return bluePrintTemplate;
    }

    public async Task<NpcEntity> GenerateNpcEntityAsync(NpcType npcType, NpcSubType subType,int level = 1)
    {

        NpcStatEntity npcStats;

        var npc = new NpcEntity()
        {
            Id = Guid.NewGuid(),
            Alignment =
                npcType.ToString().ToLower().StartsWith("animal")
                    ? NpcAlignmentType.Good
                    : NpcAlignmentType.Good.RandomEnumValue(),
            Gold = npcType.ToString().ToLower().StartsWith("animal") ? 0 : RandomUtils.Range(1, 50) * level,
            TileId = 0,
            Type = npcType.Id,
            SubType = subType.Id
        };

        if (npcType.ToString().ToLower().StartsWith("animal"))
        {
            npc.Name = _namesService.RandomAnimalName;
            npc.Gold = 0;
            npcStats = new NpcStatEntity
            {
                NpcId = npc.Id,
                Level = level,
                Health = 1,
                MaxHealth = 1,
                MaxMana = 1,
                Dexterity = 1,
                Experience = 0,
                Intelligence = 1,
                Mana = 1,
                Strength = 1,
            };
        }
        else
        {
            npc.Name = _namesService.RandomName;
            var life = RandomUtils.Range(5, 10) * level;
            var magicLife = RandomUtils.Range(5, 10) * level;
            npc.Gold = RandomUtils.Range(10, 15) * level;
            npcStats = new NpcStatEntity
            {
                NpcId = npc.Id,
                Level = level,
                Health = life,
                MaxHealth = life,
                Mana = magicLife,
                MaxMana = magicLife,
            };
        }
        await Engine.DatabaseService.InsertAsync(npc);
        await Engine.DatabaseService.InsertAsync(npcStats);

        return npc;
    }

    public async Task<NpcGameObject> GenerateNpcGameObjectAsync(
        PointPosition position, NpcType npcType, NpcSubType subType, int level = 1
    )
    {
        var entity = await GenerateNpcEntityAsync(npcType, subType, level);

        var gameObject = new NpcGameObject(position.ToPoint())
        {
            ObjectId = entity.Id,
            Name = entity.Name,
            IsTransparent = false,
            IsWalkable = true,
            Tile = entity.TileId,
        };

        return gameObject;

    }

    public async Task<GameObjectEntity> GenerateWorldGameObjectAsync(GameObjectType type)
    {
        // First of all, i search if gameObject exists
        var entities = await Engine.DatabaseService.QueryAsListAsync<GameObjectEntity>(objectEntity => objectEntity.GameObjectType == type.Id);
        if (!entities.Any())
        {
            throw new Exception($"Can't find game object type: {type}!");
        }

        return entities.RandomItem();

        //var entity = new GameObjectEntity();
    }

    public async Task<WorldGameObject> GenerateWorldGameObjectAsync(GameObjectType type, PointPosition position)
    {
        var entity = await GenerateWorldGameObjectAsync(type);

        var gameObject = new WorldGameObject(position.ToPoint())
        {
            ObjectId = entity.Id,
            IsTransparent = false,
            IsWalkable = false,
            Type = entity.GameObjectType,
            Tile = entity.TileId
        };

        return gameObject;
    }
}
