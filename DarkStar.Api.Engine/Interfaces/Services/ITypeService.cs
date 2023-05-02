using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Services.Base;
using DarkStar.Api.World.Types.GameObjects;
using DarkStar.Api.World.Types.Items;
using DarkStar.Api.World.Types.Npc;
using DarkStar.Api.World.Types.Tiles;

namespace DarkStar.Api.Engine.Interfaces.Services;
public interface ITypeService : IDarkSunEngineService
{
    void AddTile(Tile tile);
    Tile GetTile(uint id);
    Tile GetTile(string name);
    List<Tile> SearchTiles(string name, string? category, string? subCategory);
    Tile SearchTile(string name, string? category, string? subCategory);
    List<Tile> Tiles { get; }

    List<NpcType> NpcTypes { get; }
    List<NpcSubType> NpcSubTypes { get; }

    List<GameObjectType> GameObjectTypes { get; }
    GameObjectType AddGameObjectType(string name);
    GameObjectType AddGameObjectType(short id, string name);
    GameObjectType GetGameObjectType(string name);
    GameObjectType GetGameObjectType(short id);
    GameObjectType SearchGameObject(string name);

    NpcType AddNpcType(string name);
    NpcType AddNpcType(short id, string name);
    NpcSubType AddNpcSubType(string npcType, string name);
    NpcSubType AddNpcSubType(string npcType, short id, string name);
    NpcSubType GetNpcSubType(string name);
    NpcSubType GetNpcSubType(short id);
    NpcType? GetNpcType(string name);
    NpcType GetNpcType(short id);
    void AddNpcTypeTile(NpcType npcType, NpcSubType npcSubType, string tile);

    Tile GetTileForNpc(NpcType npcType, NpcSubType npcSubType);
    Tile GetTileForNpc(string npcType, string npcSubType);

    ItemType AddItemType(string name);
    ItemType AddItemType(short id, string name);
    ItemType SearchItemType(string name);

    ItemCategoryType AddItemCategoryType(string name);
    ItemCategoryType AddItemCategoryType(short id, string name);
    ItemCategoryType SearchItemCategoryType(string name);

}
