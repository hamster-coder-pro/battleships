namespace Battleships.Console;

public interface IGameManager
{
    void Shoot(
        GameInfo gameInfo,
        int row,
        int column
    );

    void Reveal(IEnumerable<SquareInfo> squareEnumerable);
}