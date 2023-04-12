using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DarkSun.Api.Attributes.Services;
using DarkSun.Api.Data.Config;
using DarkSun.Api.Engine.Interfaces.Services;
using DarkSun.Api.Utils;
using DarkSun.Api.World.Types.Tiles;
using DarkSun.Api.World.Types.Utils;
using DarkSun.Database.Entities.Base;
using DarkSun.Database.Entities.Races;
using DarkSun.Engine.Services.Base;
using Microsoft.Extensions.Logging;

namespace DarkSun.Engine.Services;

[DarkSunEngineService(nameof(BlueprintService), 2)]
public class BlueprintService : BaseService<BlueprintService>, IBlueprintService
{
    
    private readonly HashSet<RaceEntity> _racesSeed = new();
  


    public BlueprintService(ILogger<BlueprintService> logger, DirectoriesConfig directoriesConfig) : base(logger)
    {
       
    }

    protected override async ValueTask<bool> StartAsync()
    {
        return true;
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
            TileId = tileId.ParseTileType(),
        });
    }

    


}
