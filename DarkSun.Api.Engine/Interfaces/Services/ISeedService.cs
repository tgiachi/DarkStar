using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSun.Api.Engine.Interfaces.Services.Base;
using DarkSun.Api.World.Types.GameObjects;
using DarkSun.Api.World.Types.Tiles;
using DarkSun.Database.Entities.Base;

namespace DarkSun.Api.Engine.Interfaces.Services
{
    public interface ISeedService : IDarkSunEngineService
    {
        void AddRaceToSeed(string race, string description, short tileId, BaseStatEntity stat);
        void AddGameObjectToSeed(string name, string description, TileType tileType, GameObjectType gameObjectType);
    }
}
