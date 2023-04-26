using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Services.Base;
using DarkStar.Api.World.Types.GameObjects;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Api.World.Types.Tiles;

namespace DarkStar.Api.Engine.Interfaces.Services;
public interface ITypeService : IDarkSunEngineService
{
    void AddTile(Tile tile);
    Tile GetTile(uint id);
    Tile GetTile(string name);
    List<Tile> SearchTiles(string name, string? category, string? subCategory);
    List<Tile> Tiles { get; }

    List<GameObjectType> GameObjectTypes { get; }
    GameObjectType AddGameObjectType(string name);
    GameObjectType AddGameObjectType(short id, string name);
    GameObjectType GetGameObjectType(string name);
    GameObjectType GetGameObjectType(short id);

    NpcType AddNpcType(string name);
    NpcType AddNpcType(short id, string name);
    NpcSubType AddNpcSubType(string npcType, string name);
    NpcSubType AddNpcSubType(string npcType, short id, string name);
    NpcType? GetNpcType(string name);

}
