using GoRogue.MapGeneration;

namespace DarkStar.Api.Engine.Map.Enums;

public enum MapGeneratorType : short
{
    RectangleMap = 0,
    CellularAutomata = 1,
    Dungeon = 2,
    BasicRandomRooms
}
