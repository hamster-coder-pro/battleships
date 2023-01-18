namespace Battleships.Console;

public static class CollectionExtensions
{
    public static void Shuffle<T>(this IList<T> list, Random random)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static IList<T> AsList<T>(this IEnumerable<T> source)
    {
        if (source is IList<T> result)
        {
            return result;
        }

        return source.ToList();
    }
}