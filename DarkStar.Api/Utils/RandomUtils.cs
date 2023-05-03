using System;
using System.Numerics;

namespace DarkStar.Api.Utils;

public static class RandomUtils
{
    private static readonly Random s_random = new();

    public static int Range(int min, int max) => s_random.Next(min, max);

    public static float Range(float min, float max) => (float)s_random.NextDouble() * (max - min) + min;

    public static bool RandomBool() => s_random.Next(2) == 1;

    public static IEnumerable<int> RandEnumerable(int min, int max) => Enumerable.Range(min, Range(min, max));
}
