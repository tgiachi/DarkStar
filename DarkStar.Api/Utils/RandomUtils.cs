
using System.Numerics;

namespace DarkStar.Api.Utils;

public static class RandomUtils
{
    private static readonly Random s_random = new();

    public static int Range(int min, int max)
    {
        return s_random.Next(min, max);
    }

    public static float Range(float min, float max)
    {
        return (float)s_random.NextDouble() * (max - min) + min;
    }

    public static bool RandomBool()
    {
        return Range(0, 1) == 1;
    }

    public static Vector3 RandomVector3(Vector3 min, Vector3 max)
    {
        return new Vector3
        {
            X = Range((int)min.X, (int)max.X),
            Y = Range((int)min.Y, (int)max.Y),
            Z = Range((int)min.Z, (int)max.Z)
        };
    }

    public static IEnumerable<int> RandEnumerable(int min, int max)
    {
        return Enumerable.Range(min, Range(min, max));
    }



    public static T RandomEnumValue<T>() where T : Enum
    {
        var v = Enum.GetValues(typeof(T));
        return (T)v.GetValue(s_random.Next(v.Length))!;
    }
}
