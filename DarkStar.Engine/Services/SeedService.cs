using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DarkStar.Api.Attributes.Seed;
using DarkStar.Api.Attributes.Services;
using Microsoft.Extensions.Logging;
using DarkStar.Api.Utils;
using DarkStar.Api.Serialization.TileSets;
using DarkStar.Api.World.Types.GameObjects;
using DarkStar.Api.Engine.Serialization.Seeds;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Engine.Services.Base;
using DarkStar.Api.Data.Config;
using DarkStar.Api.Engine.Serialization;
using DarkStar.Database.Entities.Races;
using DarkStar.Database.Entities.Base;
using DarkStar.Database.Entities.Item;
using DarkStar.Database.Entities.TileSets;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Database.Entities.Objects;
using FastEnumUtility;
using DarkStar.Api.Serialization.Types;
using DarkStar.Api.World.Types.Items;

namespace DarkStar.Engine.Services;

[DarkStarEngineService(nameof(SeedService), 8)]
public class SeedService : BaseService<SeedService>, ISeedService
{
    private readonly HashSet<RaceEntity> _racesSeed = new();
    private readonly HashSet<GameObjectEntity> _gameObjectSeed = new();
    private readonly HashSet<ItemEntity> _itemsSeed = new();
    private readonly HashSet<TextContentEntity> _textContentSeed = new();

    private readonly DirectoriesConfig _directoriesConfig;
    private readonly ITypeService _typeService;

    public SeedService(
        ILogger<SeedService> logger, DirectoriesConfig directoriesConfig, ITypeService typeService
    ) : base(logger)
    {
        _directoriesConfig = directoriesConfig;
        _typeService = typeService;
    }

    protected override async ValueTask<bool> StartAsync()
    {
        await CheckSeedTemplatesAsync();
        await CheckSeedDirectoriesAsync();
        await ScanTileSetsAsync();
        await InsertDbSeedsAsync();
        await LoadCsvSeedsAsync();

        return true;
    }

    private async Task LoadCsvSeedsAsync()
    {
        Logger.LogInformation("Loading seeds");
        //await LoadSeedAsync<TileSetMapSerializable>();
        await LoadSeedAsync<GameObjectTypeSerializableEntity>();
        await LoadSeedAsync<NpcTypeAndSubTypeSerializableEntity>();
        await LoadSeedAsync<WorldObjectSeedEntity>();
        await LoadSeedAsync<RaceObjectSeedEntity>();
        await LoadSeedAsync<ItemObjectSeedEntity>();
        await LoadSeedAsync<ItemDropObjectSeedEntity>();
    }

    private async Task CheckSeedTemplatesAsync()
    {
        Logger.LogInformation("Checking Seed Templates");

        await CheckSeedTemplateAsync<GameObjectTypeSerializableEntity>();
        await CheckSeedTemplateAsync<NpcTypeAndSubTypeSerializableEntity>();
        await CheckSeedTemplateAsync<ItemDropObjectSeedEntity>();
        await CheckSeedTemplateAsync<ItemObjectSeedEntity>();
        await CheckSeedTemplateAsync<WorldObjectSeedEntity>();
        await CheckSeedTemplateAsync<RaceObjectSeedEntity>();
        //await CheckSeedTemplateAsync<TileSetMapSerializable>();

        // await CheckSeedTemplateAsync(GetDefaultTileSetMap());
    }

    /*private IEnumerable<TileSetMapSerializable> GetDefaultTileSetMap()
    {
        return FastEnum.GetValues<TileType>().OrderBy(k => (short)k).Select(s =>
            new TileSetMapSerializable() { GameObjectType = s, Id = (short)s, IsTransparent = false });
    }*/

    private Task CheckSeedDirectoriesAsync()
    {
        var attributes = AssemblyUtils.GetAttribute<SeedObjectAttribute>();
        foreach (var dir in attributes.Select(
                         type =>
                             type.GetCustomAttribute<SeedObjectAttribute>()!
                     )
                     .Select(attr => Path.Join(_directoriesConfig[DirectoryNameType.Seeds], attr.TemplateDirectory))
                     .Where(dir => !Directory.Exists(dir)))
        {
            Directory.CreateDirectory(dir);
        }

        return Task.CompletedTask;
    }


    private async Task LoadSeedAsync<TEntity>() where TEntity : class, new()
    {
        var attribute = typeof(TEntity).GetCustomAttribute<SeedObjectAttribute>();
        var directory = Path.Join(_directoriesConfig[DirectoryNameType.Seeds], attribute!.TemplateDirectory);
        var files = Directory.GetFiles(directory, "*.csv");

        Logger.LogInformation("Found {Files} for {GameObjectType} seed", files.Length, attribute.TemplateDirectory);
        foreach (var file in files)
        {
            await LoadSeedFileAsync<TEntity>(file);
        }
    }

    public void AddGameObjectSeed(string name, string description, string tileNAme, string gameObjectName, object data)
    {
        _gameObjectSeed.Add(
            new GameObjectEntity()
            {
                Name = name,
                Description = description,
                TileId = _typeService.SearchTile(tileNAme, null, null)!.Id,
                GameObjectType = _typeService.SearchGameObject(gameObjectName)!.Id,
                Data = JsonSerializer.Serialize(data)
            }
        );
    }

    public void AddTextContentSeed(string name, string content)
    {
        _textContentSeed.Add(
            new TextContentEntity
            {
                Name = name.ToUpper(),
                Content = content
            }
        );
    }

    private async Task LoadSeedFileAsync<TEntity>(string fileName) where TEntity : class, new()
    {
        if (typeof(TEntity) == typeof(WorldObjectSeedEntity))
        {
            var gameObjects = await SeedCsvParser.Instance.ParseAsync<WorldObjectSeedEntity>(fileName);
            gameObjects.ToList()
                .ForEach(
                    go => _gameObjectSeed.Add(
                        new GameObjectEntity()
                        {
                            Name = go.Name,
                            Description = go.Description,
                            TileId = _typeService.SearchTile(go.TileName, null, null)!.Id,
                            GameObjectType = _typeService.SearchGameObject(go.GameObjectName)!.Id,
                            Data = JsonSerializer.Serialize(go.Data)
                        }
                    )
                );
        }

        if (typeof(TEntity) == typeof(GameObjectTypeSerializableEntity))
        {
            var gameObjectsTypes = await SeedCsvParser.Instance.ParseAsync<GameObjectTypeSerializableEntity>(fileName);

            foreach (var gameObjectType in gameObjectsTypes)
            {
                _typeService.AddGameObjectType(gameObjectType.Name);
            }
        }

        if (typeof(TEntity) == typeof(NpcTypeAndSubTypeSerializableEntity))
        {
            var npcTypes = await SeedCsvParser.Instance.ParseAsync<NpcTypeAndSubTypeSerializableEntity>(fileName);

            foreach (var npcType in npcTypes)
            {
                _typeService.AddNpcSubType(npcType.NpcName, npcType.NpcSubTypeName);

                _typeService.AddNpcTypeTile(
                    _typeService.GetNpcType(npcType.NpcName).Value,
                    _typeService.GetNpcSubType(npcType.NpcSubTypeName),
                    npcType.TileName
                );
            }
        }

        if (typeof(TEntity) == typeof(ItemObjectSeedEntity))
        {
            var itemsTypes = await SeedCsvParser.Instance.ParseAsync<ItemObjectSeedEntity>(fileName);
            foreach (var importItem in itemsTypes)
            {
                var itemEntity = new ItemEntity
                {
                    Name = importItem.Name,
                    Description = importItem.Description,
                    EquipLocation = importItem.EquipLocation,
                    Attack = importItem.Attack,
                    Defense = importItem.Defense,
                    Weight = importItem.Weight,
                    MinLevel = importItem.MinLevel,
                    Speed = importItem.Speed,
                    BuyDice = importItem.BuyDice,
                    SellDice = importItem.SellDice,
                    TileType = _typeService.SearchTile(importItem.TileName, null, null).Id,
                    ItemRarity = importItem.ItemRarity,
                    Category = _typeService.AddItemCategoryType(importItem.Category),
                    Type = _typeService.AddItemType(importItem.Type)
                };

                await Engine.DatabaseService.InsertAsync(itemEntity);
            }
        }
    }


    private async Task CheckSeedTemplateAsync<TEntity>(IEnumerable<TEntity>? defaultData = null) where TEntity : class, new()
    {
        Logger.LogInformation("Checking Seed Template for type: {GameObjectType}", typeof(TEntity).Name);
        var fileName = Path.Join(
            _directoriesConfig[DirectoryNameType.SeedTemplates],
            $"{typeof(TEntity).Name.Replace("Entity", "").Replace("Serializable", "").ToUnderscoreCase()}.csv"
        );
        if (!File.Exists(fileName))
        {
            if (defaultData == null)
            {
                defaultData = Enumerable.Empty<TEntity>();
            }

            await SeedCsvParser.Instance.WriteHeaderToFileAsync(fileName, defaultData);
        }
    }

    public void AddRaceToSeed(string race, string description, short tileId, BaseStatEntity stat)
    {
        Logger.LogInformation("Adding Race {Race} to seed", race);
        _racesSeed.Add(
            new RaceEntity()
            {
                Name = race,
                Description = description,
                Dexterity = stat.Dexterity,
                Intelligence = stat.Intelligence,
                Luck = stat.Luck,
                Strength = stat.Strength,
                TileId = tileId
            }
        );
    }

    public void AddGameObjectToSeed(
        string name, string description, int tileType,
        GameObjectType gameObjectType
    )
    {
        _gameObjectSeed.Add(
            new GameObjectEntity()
            {
                Name = name,
                Description = description,
                TileId = (uint)tileType,
                GameObjectType = gameObjectType.Id
            }
        );
    }

    private async Task ScanTileSetsAsync()
    {
        var files = Directory.GetFiles(
            _directoriesConfig[DirectoryNameType.Assets],
            "*.tileset",
            SearchOption.AllDirectories
        );
        Logger.LogInformation("Scanning TileSets");
        foreach (var tileSet in files)
        {
            await LoadTileSetDefinitionAsync(tileSet);
        }
    }

    private async Task InsertDbSeedsAsync()
    {
        foreach (var go in _gameObjectSeed)
        {
            var gameObject =
                await Engine.DatabaseService.QueryAsSingleAsync<GameObjectEntity>(entity => entity.Name == go.Name);
            if (gameObject == null!)
            {
                await Engine.DatabaseService.InsertAsync(go);
            }
            else
            {
                gameObject.Name = go.Name;
                gameObject.Description = go.Description;
                gameObject.TileId = go.TileId;
                gameObject.GameObjectType = go.GameObjectType;
                gameObject.Data = go.Data;
                await Engine.DatabaseService.UpdateAsync(gameObject);
            }
        }

        foreach (var contentEntity in _textContentSeed)
        {
            var existTextSeed =
                await Engine.DatabaseService.QueryAsSingleAsync<TextContentEntity>(
                    entity => entity.Name == contentEntity.Name
                );
            if (existTextSeed == null!)
            {
                await Engine.DatabaseService.InsertAsync(contentEntity);
            }
            else
            {
                existTextSeed.Content = contentEntity.Content;
                await Engine.DatabaseService.UpdateAsync(existTextSeed);
            }
        }
    }

    private async Task LoadTileSetDefinitionAsync(string tileSet)
    {
        var tileSetDefinition = JsonSerializer.Deserialize<TileSetSerializableEntity>(await File.ReadAllTextAsync(tileSet));
        Logger.LogInformation("Loading TileSet {TileSet}", tileSetDefinition!.Name);


        var tilesDirectory = new FileInfo(tileSet);
        var tileSetImageInfo = new FileInfo(Path.Join(tilesDirectory.DirectoryName, tileSetDefinition.Source));

        var tileEntity =
            await Engine.DatabaseService.QueryAsSingleAsync<TileSetEntity>(entity => entity.Name == tileSetDefinition!.Name);

        if (tileEntity == null!)
        {
            tileEntity = new TileSetEntity()
            {
                Name = tileSetDefinition!.Name,
                Source = Path.Join(tilesDirectory.DirectoryName!, tileSetDefinition!.Source),
                FileSize = tileSetImageInfo.Length,
                TileHeight = tileSetDefinition!.TileHeight,
                TileWidth = tileSetDefinition!.TileWidth,
                TileSetMapFileName = Path.Join(tilesDirectory.DirectoryName!, tileSetDefinition!.TileSetMapFileName),
            };
            await Engine.DatabaseService.InsertAsync(tileEntity);
        }

        var tileMap =
            await SeedCsvParser.Instance.ParseAsync<TileSetMapSerializable>(
                Path.Join(
                    tilesDirectory.DirectoryName!,
                    tileSetDefinition!.TileSetMapFileName
                )
            );

        foreach (var tile in tileMap)
        {
            var tileMapEntity = await Engine.DatabaseService.QueryAsSingleAsync<TileSetMapEntity>(
                entity => entity.TileSetId == tileEntity.Id && entity.TileId == tile.Id
            );
            if (tileMapEntity != null!)
            {
                continue;
            }

            tileMapEntity = new TileSetMapEntity()
            {
                TileSetId = tileEntity.Id,
                TileId = tile.Id,
                Category = tile.Category,
                Tag = tile.Tag,
                IsTransparent = tile.IsTransparent,
                SubCategory = tile.SubCategory,
                Name = tile.Name

                //TileType = tile.GameObjectType,
                //IsTransparent = tile.IsTransparent
            };
            await Engine.DatabaseService.InsertAsync(tileMapEntity);

            _typeService.AddTile(
                new Tile(
                    tile.Name,
                    tile.Id,
                    tile.Category,
                    tile.SubCategory,
                    tile.IsTransparent,
                    string.IsNullOrEmpty(tile.Tag) ? null : tile.Tag
                )
            );
        }
    }
}
