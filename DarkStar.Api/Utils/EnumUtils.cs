using FastEnumUtility;

namespace DarkStar.Api.Utils;

public static class EnumUtils
{
    private static readonly Random s_random = new();

    public static TEnum RandomEnumValue<TEnum>(this TEnum _) where TEnum : struct, Enum
    {
        var v = FastEnum.GetValues<TEnum>();
        return v[s_random.Next(v.Count)];
    }

    public static List<T> SearchType<T>(this T _, string tileType) where T : struct, Enum
    {
        return FastEnum.GetValues<T>().Where(s => s.ToString().ToLower().StartsWith(tileType.ToLower()))
            .ToList();
    }
}
