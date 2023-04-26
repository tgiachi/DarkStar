using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Data.Config;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Utils;
using DarkStar.Engine.Services.Base;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DarkStar.Engine.Services;

[DarkStarEngineService(nameof(NamesService), 9)]
public class NamesService : BaseService<NamesService>, INamesService
{
    private const string RandomGeneratorBaseUrl =
        "https://donjon.bin.sh/name/rpc-name.fcgi?type={0}&n=10&as_json=1";

    private const string RandomAnimalNameGeneratorUrl =
        "https://story-shack-cdn-v2.glitch.me/generators/{0}-name-generator/{1}?count=10";

    public string RandomCityName => _citiesNames.RandomItem();
    public string RandomAnimalName => _animalNames.RandomItem();
    public string RandomName => _names.RandomItem();

    private readonly string _cacheDirectory;
    private readonly List<string> _animalNames = new();
    private readonly List<string> _citiesNames = new();
    private readonly List<string> _names = new();

    public NamesService(ILogger<NamesService> logger, DirectoriesConfig directoriesConfig) : base(logger)
    {
        _cacheDirectory = directoriesConfig[DirectoryNameType.Cache];
    }

    protected override async ValueTask<bool> StartAsync()
    {
        await PrepareCacheAsync();

        return true;
    }

    private async Task PrepareCacheAsync()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        Logger.LogInformation("Building cache");
        _names.AddRange(await DownloadLinkAsync("Human Male") ?? new List<string>());
        _names.AddRange(await DownloadLinkAsync("Human Female") ?? new List<string>());
        _names.AddRange(await DownloadLinkAsync("Dwarvish Male") ?? new List<string>());
        _names.AddRange(await DownloadLinkAsync("Dwarvish Female") ?? new List<string>());
        _names.AddRange(await DownloadLinkAsync("Elvish Male") ?? new List<string>());
        _names.AddRange(await DownloadLinkAsync("Elvish Female") ?? new List<string>());
        _animalNames.AddRange(await DownloadLinkAsync("cat", "male") ?? new List<string>());
        _animalNames.AddRange(await DownloadLinkAsync("cat", "female") ?? new List<string>());
        _citiesNames.AddRange(await DownloadLinkAsync("Town") ?? new List<string>());

        var distinctNames = _names.Distinct().ToList();
        _names.Clear();
        _names.AddRange(distinctNames);
        stopWatch.Stop();

        Logger.LogInformation("Cache built in {Time} ms", stopWatch.ElapsedMilliseconds);
    }

    private async Task<List<string>?> DownloadLinkAsync(string animalType, string sex, int count = 10)
    {
        var link = GetAnimalLink(animalType, sex);
        var fileName = $"{link.Sha1Hash()}.json";
        var fullList = new List<string>();
        if (File.Exists(Path.Join(_cacheDirectory, fileName)))
        {
            return JsonSerializer.Deserialize<List<string>>(
                await File.ReadAllTextAsync(Path.Join(_cacheDirectory, fileName)));
        }

        using var httpClient = new HttpClient();
        Logger.LogInformation("Downloading animals {Sex} {GameObjectType} ", sex, animalType);
        for (var i = 0; i < count; i++)
        {
            var json = await httpClient.GetStringAsync(link);
            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<AnimalList>(json);
            fullList.AddRange(list!.data.Select(s => s.name));
        }

        Logger.LogInformation("Downloading animals {Sex} {GameObjectType} OK", sex, animalType);

        await File.WriteAllTextAsync(Path.Join(_cacheDirectory, fileName), JsonSerializer.Serialize(fullList));

        return fullList;
    }

    private async Task<List<string>?> DownloadLinkAsync(string category, int count = 10)
    {
        var link = GetLink(category);
        var fileName = $"{link.Sha1Hash()}.json";
        var fullList = new List<string>();
        if (File.Exists(Path.Join(_cacheDirectory, fileName)))
        {
            return JsonSerializer.Deserialize<List<string>>(
                await File.ReadAllTextAsync(Path.Join(_cacheDirectory, fileName)));
        }

        using var httpClient = new HttpClient();
        Logger.LogInformation("Downloading category: {Category}", category);
        for (var i = 0; i < count; i++)
        {
            var list = await httpClient.GetFromJsonAsync<List<string>>(link);
            fullList.AddRange(list!);
        }

        Logger.LogInformation("Downloading category: {Category} OK", category);

        await File.WriteAllTextAsync(Path.Join(_cacheDirectory, fileName), JsonSerializer.Serialize(fullList));

        return fullList;
    }

    private string GetLink(string category) => string.Format(RandomGeneratorBaseUrl, category);

    private string GetAnimalLink(string animalType, string sex) => string.Format(RandomAnimalNameGeneratorUrl, animalType, sex);

    public class AnimalList
    {
        public List<AnimalName> data { get; set; }
    }

    public class AnimalName
    {
        public string name { get; set; }
    }
}
