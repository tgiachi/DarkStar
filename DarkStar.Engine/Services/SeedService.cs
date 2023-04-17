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
using DarkStar.Api.World.Types.Utils;
using DarkStar.Api.Data.Config;
using DarkStar.Api.Engine.Serialization;
using DarkStar.Database.Entities.Races;
using DarkStar.Database.Entities.Base;
using DarkStar.Database.Entities.Item;
using DarkStar.Database.Entities.TileSets;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Database.Entities.Objects;
using FastEnumUtility;

namespace DarkStar.Engine.Services
{
    [DarkStarEngineService(nameof(SeedService), 8)]
    public class SeedService : BaseService<SeedService>, ISeedService
    {
        private readonly HashSet<RaceEntity> _racesSeed = new();
        private readonly HashSet<GameObjectEntity> _gameObjectSeed = new();
        private readonly HashSet<ItemEntity> _itemsSeed = new();

        private readonly DirectoriesConfig _directoriesConfig;

        public SeedService(ILogger<SeedService> logger, DirectoriesConfig directoriesConfig) : base(logger)
        {
            _directoriesConfig = directoriesConfig;
        }

        protected override async ValueTask<bool> StartAsync()
        {
            await CheckSeedTemplatesAsync();
            await CheckSeedDirectoriesAsync();
            await LoadCsvSeedsAsync();
            await ScanTileSetsAsync();
            return true;
        }

        private async Task LoadCsvSeedsAsync()
        {
            Logger.LogInformation("Loading seeds");
            await LoadSeedAsync<WorldObjectSeedEntity>();
            await LoadSeedAsync<RaceObjectSeedEntity>();
            await LoadSeedAsync<ItemObjectSeedEntity>();
            await LoadSeedAsync<ItemDropObjectSeedEntity>();
        }

        private async Task CheckSeedTemplatesAsync()
        {
            Logger.LogInformation("Checking Seed Templates");

            await CheckSeedTemplateAsync<ItemDropObjectSeedEntity>();
            await CheckSeedTemplateAsync<ItemObjectSeedEntity>();
            await CheckSeedTemplateAsync<WorldObjectSeedEntity>();
            await CheckSeedTemplateAsync<RaceObjectSeedEntity>();
            await CheckSeedTemplateAsync<TileSetMapSerializable>(GetDefaultTileSetMap());
        }

        private IEnumerable<TileSetMapSerializable> GetDefaultTileSetMap()
        {
            return FastEnum.GetValues<TileType>().OrderBy(k => (short)k).Select(s =>
                new TileSetMapSerializable() { Type = s, Id = (short)s, IsBlocked = false });
        }

        private Task CheckSeedDirectoriesAsync()
        {
            var attributes = AssemblyUtils.GetAttribute<SeedObjectAttribute>();
            foreach (var dir in attributes.Select(type =>
                             type.GetCustomAttribute<SeedObjectAttribute>()!)
                         .Select(attr => Path.Join(_directoriesConfig[DirectoryNameType.Seeds], attr.TemplateDirectory))
                         .Where(dir => !Directory.Exists(dir)))
            {
                Directory.CreateDirectory(dir);
            }

            return Task.CompletedTask;
        }


        private Task LoadSeedAsync<TEntity>() where TEntity : class, new()
        {
            var attribute = typeof(TEntity).GetCustomAttribute<SeedObjectAttribute>();
            var directory = Path.Join(_directoriesConfig[DirectoryNameType.Seeds], attribute!.TemplateDirectory);
            var files = Directory.GetFiles(directory, "*.csv");

            Logger.LogInformation("Found {Files} for {Type} seed", files.Length, attribute.TemplateDirectory);

            return Task.CompletedTask;
        }

        private async Task CheckSeedTemplateAsync<TEntity>(IEnumerable<TEntity>? defaultData = null) where TEntity : class, new()
        {
            Logger.LogInformation("Checking Seed Template for type: {Type}", typeof(TEntity).Name);
            var fileName = Path.Join(_directoriesConfig[DirectoryNameType.SeedTemplates],
                $"{typeof(TEntity).Name.Replace("Entity", "").ToUnderscoreCase()}.csv");
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
            _racesSeed.Add(new RaceEntity()
            {
                Name = race,
                Description = description,
                Dexterity = stat.Dexterity,
                Intelligence = stat.Intelligence,
                Luck = stat.Luck,
                Strength = stat.Strength,
                TileId = tileId.ParseTileType()
            });
        }

        public void AddGameObjectToSeed(string name, string description, TileType tileType,
            GameObjectType gameObjectType)
        {
            _gameObjectSeed.Add(new GameObjectEntity()
            {
                Name = name,
                Description = description,
                TileId = tileType,
                Type = gameObjectType
            });
        }

        private async Task ScanTileSetsAsync()
        {
            var files = Directory.GetFiles(_directoriesConfig[DirectoryNameType.Assets], "*.tileset");
            Logger.LogInformation("Scanning TileSets");
            foreach (var tileSet in files)
            {
                await LoadTileSetDefinitionAsync(tileSet);
            }
        }

        private async Task LoadTileSetDefinitionAsync(string tileSet)
        {
            var tileSetDefinition = JsonSerializer.Deserialize<TileSetSerializableEntity>(await File.ReadAllTextAsync(tileSet));
            var tilesDirectory = new FileInfo(tileSet);
            var tileEntity = await Engine.DatabaseService.QueryAsSingleAsync<TileSetEntity>(entity => entity.Name == tileSetDefinition!.Name);

            if (tileEntity == null!)
            {
                tileEntity = new TileSetEntity()
                {
                    Name = tileSetDefinition!.Name,
                    Source = tileSetDefinition!.Source,
                    TileHeight = tileSetDefinition!.TileHeight,
                    TileWidth = tileSetDefinition!.TileWidth,
                    TileSetMapFileName = Path.Join(tilesDirectory.DirectoryName!, tileSetDefinition!.TileSetMapFileName),
                };
                await Engine.DatabaseService.InsertAsync(tileEntity);
            }

            var tileMap =
                await SeedCsvParser.Instance.ParseAsync<TileSetMapSerializable>(Path.Join(tilesDirectory.DirectoryName!,
                    tileSetDefinition!.TileSetMapFileName));

            foreach (var tile in tileMap)
            {
                var tileMapEntity = await Engine.DatabaseService.QueryAsSingleAsync<TileSetMapEntity>(entity => entity.TileSetId == tileEntity.Id && entity.TileId == tile.Id);
                if (tileMapEntity == null!)
                {
                    tileMapEntity = new TileSetMapEntity()
                    {
                        TileSetId = tileEntity.Id,
                        TileId = tile.Id,
                        TileType = tile.Type,
                        IsBlocked = tile.IsBlocked
                    };
                    await Engine.DatabaseService.InsertAsync(tileMapEntity);
                }
            }
        }
    }
}
