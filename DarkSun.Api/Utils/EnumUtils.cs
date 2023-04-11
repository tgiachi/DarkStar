namespace DarkSun.Api.Utils;

public static class EnumUtils
{
    private static readonly Random s_random = new();

    public static TEnum RandomEnumValue<TEnum>(this TEnum _) where TEnum : struct, Enum
    {
        var v = Enum.GetValues(typeof(TEnum));
        return (TEnum)v.GetValue(s_random.Next(v.Length))!;
    }

    public static List<T> SearchType<T>(this T _, string tileType) where T : struct, Enum
    {
        return Enum.GetValues<T>().Where(s => s.ToString().ToLower().StartsWith(tileType.ToLower()))
            .ToList();
    }
}
