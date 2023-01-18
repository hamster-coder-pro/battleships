namespace Battleships.Console;

public static class SquareInfoExtensions
{
    public static IEnumerable<SquareInfo> EnumerateNear(this IEnumerable<SquareInfo> squares, SquareInfo target)
    {
        return squares.Where(x => x.IsNear(target));
    }
}