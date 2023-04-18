using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Data.Config;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Engine.Services.Base;

using Microsoft.Extensions.Logging;
using TiledSharp;

namespace DarkStar.Engine.Services
{
    [DarkStarEngineService(nameof(BlueprintService), 2)]
    public class BlueprintService : BaseService<BlueprintService>, IBlueprintService
    {
        private readonly DirectoriesConfig _directoriesConfig;

        public BlueprintService(ILogger<BlueprintService> logger, DirectoriesConfig directoriesConfig) : base(logger)
        {
            _directoriesConfig = directoriesConfig;
        }

        protected override async ValueTask<bool> StartAsync()
        {
            await ScanForMapTemplatesAsync();

            return true;
        }

        private async ValueTask ScanForMapTemplatesAsync()
        {
            var mapTemplates = Directory.GetFiles(_directoriesConfig[DirectoryNameType.BluePrints], "*.tmx");

            foreach (var mapTemplate in mapTemplates)
            {
                await LoadMapTemplateAsync(mapTemplate);
            }
        }

        private ValueTask LoadMapTemplateAsync(string fileName)
        {
            try
            {
                var map =  new TmxMap(fileName);
                // Check if the map is valid
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load map template {FileName}: {Error}", fileName, ex);
            }
            return ValueTask.CompletedTask;
        }
    }
}
