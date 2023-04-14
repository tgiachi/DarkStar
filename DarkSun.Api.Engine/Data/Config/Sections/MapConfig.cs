

namespace DarkSun.Api.Engine.Data.Config.Sections
{
    public class MapDefaultSizes
    {
        public const int DefaultWorldWidth = 1000;
        public const int DefaultWorldHeight = 1000;
        public const int DefaultWidth = 40;
        public const int DefaultHeight = 40;
        public const int NumWorlds = 1;
        public const int NumCities = NumWorlds * 1;
        public const int NumDungeons = NumCities * 5;
    }

    public class MapConfig
    {
        public int SaveEveryMinutes { get; set; } = 1;
        public WorldMapConfig Worlds { get; set; } = new();
        public CityMapConfig Cities { get; set; } = new();
        public DungeonMapConfig Dungeons { get; set; } = new();
    }

    public class WorldMapConfig
    {
        public int Num { get; set; } = MapDefaultSizes.NumWorlds;
        public int Width { get; set; } = MapDefaultSizes.DefaultWorldWidth;
        public int Height { get; set; } = MapDefaultSizes.DefaultWorldHeight;
    }


    public class CityMapConfig
    {
        public int Num { get; set; } = MapDefaultSizes.NumCities;
        public int Width { get; set; } = MapDefaultSizes.DefaultWidth;
        public int Height { get; set; } = MapDefaultSizes.DefaultHeight;
    }

    public class DungeonMapConfig
    {
        public int Num { get; set; } = MapDefaultSizes.NumDungeons;
        public int Width { get; set; } = MapDefaultSizes.DefaultWidth;
        public int Height { get; set; } = MapDefaultSizes.DefaultHeight;
    }
}
