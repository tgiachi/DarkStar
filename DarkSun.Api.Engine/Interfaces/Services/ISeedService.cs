using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Services.Base;
using DarkStar.Api.World.Types.GameObjects;
using DarkStar.Api.World.Types.Tiles;
using DarkStar.Database.Entities.Base;

namespace DarkStar.Api.Engine.Interfaces.Services
{
    public interface ISeedService : IDarkSunEngineService
    {
        void AddRaceToSeed(string race, string description, short tileId, BaseStatEntity stat);
        void AddGameObjectToSeed(string name, string description, TileType tileType, GameObjectType gameObjectType);
    }
}
