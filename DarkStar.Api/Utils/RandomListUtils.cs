namespace DarkStar.Api.Utils;

public static class RandomListUtils
{
    private static readonly Random Random = new();

    public static TEntity RandomItem<TEntity>(this List<TEntity> list)
    {
        return list[Random.Next(list.Count)];
    }

    public static IEnumerable<TEntity> RandomItems<TEntity>(this List<TEntity> list, int num)
    {
        return list.OrderBy(arg => Guid.NewGuid()).Take(num).ToList();
    }
}
