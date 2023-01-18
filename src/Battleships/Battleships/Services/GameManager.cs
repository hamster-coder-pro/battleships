namespace Battleships.Console;

public class GameManager: IGameManager
{
    public void Reveal(GameInfo gameInfo, Battleship battleship)
    {
        var shipSquares = EnumerateBattleshipSquares(gameInfo, battleship).ToList();
        Reveal(shipSquares);
        Reveal(gameInfo.Map.Where(x => shipSquares.Any(x.IsNear)));
    }

    public IEnumerable<SquareInfo> EnumerateBattleshipSquares(GameInfo gameInfo, Battleship battleship)
    {
        return gameInfo.Map.Where(x => x.Battleship == battleship);
    }

    public SquareInfo FindSquare(
        GameInfo gameInfo,
        int row,
        int column
    )
    {
        var result = gameInfo.Map.FirstOrDefault(x => x.Row == row && x.Column == column);
        if (result == null) throw new ArgumentOutOfRangeException("Wrong coordinates", default(Exception));
        return result;
    }

    public void Reveal(SquareInfo square)
    {
        square.IsShown = true;
    }

    public void Reveal(IEnumerable<SquareInfo> squares)
    {
        foreach (var square in squares)
        {
            Reveal(square);
        }
    }

    public void Shoot(
        GameInfo gameInfo,
        int row,
        int column
    )
    {
        var square = FindSquare(gameInfo, row, column);

        if (square.IsShown == false)
        {
            square.IsShown = true;
            gameInfo.Shots += 1;
            if (square.Battleship != null)
            {
                gameInfo.Hits += 1;
                var shipSquares = gameInfo.Map.Where(x => x.Battleship == square.Battleship).ToArray();
                if (shipSquares.All(x => x.IsShown))
                {
                    square.Battleship.IsSinked = true;

                    Reveal(gameInfo, square.Battleship);

                    gameInfo.Sinks += 1;
                }
            }
            else
            {
                gameInfo.Misses += 1;
            }
        }
    }
}