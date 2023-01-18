namespace Battleships.Console;

internal class GameDesigner: IGameDesigner
{
    private static readonly Random Random;

    static GameDesigner()
    {
        Random = new Random();
    }

    public IEnumerable<Battleship> CreateBattleships(IEnumerable<BattleshipRequest> requests)
    {
        foreach (var battleshipRequest in requests)
        {
            for (int battleShip = 0; battleShip < battleshipRequest.Count; battleShip++)
            {
                yield return new Battleship()
                {
                    IsSinked = false, Size = battleshipRequest.Size
                };
            }
        }
    }

    public IEnumerable<SquareInfo> CreateMap(int mapWidth, int mapHeight)
    {
        for (int x = 1; x <= mapWidth; x++)
        {
            for (int y = 1; y <= mapHeight; y++)
            {
                yield return new SquareInfo() { Column = x, Row = y };
            }
        }
    }

    public IEnumerable<SquareInfo> CreateMap(
        int mapWidth,
        int mapHeight,
        IEnumerable<Battleship> battleships
    )
    {
        var battleshipList = battleships.OrderByDescending(x => x.Size).ToList();

        var result = CreateMap(mapWidth, mapHeight).ToList();

        var direction = Enumerable.Range(0, 4).AsList();

        foreach (var battleship in battleshipList)
        {
            var freeSquares = result.Where(x => x.IsBusy == false).ToList();

            if (freeSquares.Count < battleship.Size)
            {
                throw new Exception("Size of the board is not enough to place all ships");
            }

            var shipSquares = new List<SquareInfo>();
            do
            {
                var initSquare = freeSquares[Random.Next(freeSquares.Count)];
                direction.Shuffle(Random);

                foreach (var dir in direction)
                {
                    switch (dir)
                    {
                        case 0:
                            shipSquares = freeSquares.Where(x => x.Row >= initSquare.Row && x.Row < initSquare.Row + battleship.Size && x.Column == initSquare.Column).ToList();
                            break;
                        case 1:
                            shipSquares = freeSquares.Where(x => x.Column >= initSquare.Column && x.Column < initSquare.Column + battleship.Size && x.Row == initSquare.Row).ToList();
                            break;
                        case 2:
                            shipSquares = freeSquares.Where(x => x.Row <= initSquare.Row && x.Row > initSquare.Row - battleship.Size && x.Column == initSquare.Column).ToList();
                            break;
                        case 3:
                            shipSquares = freeSquares.Where(x => x.Column <= initSquare.Column && x.Column > initSquare.Column - battleship.Size && x.Row == initSquare.Row).ToList();
                            break;
                    }

                    if (shipSquares.Count == battleship.Size)
                    {
                        break;
                    }
                }
            } while (shipSquares.Count != battleship.Size);

            foreach (var shipSquare in shipSquares)
            {
                // assign ship to ship square
                shipSquare.Battleship = battleship;
                shipSquare.IsBusy = true;

                // mark near squares as busy
                foreach (var square in freeSquares.EnumerateNear(shipSquare))
                {
                    square.IsBusy = true;
                }
            }
        }

        return result;
    }

    public GameInfo GenerateMap(
        int width,
        int height,
        IEnumerable<BattleshipRequest> battleshipRequests
    )
    {
        var result = new GameInfo();
        result.Battleships = CreateBattleships(battleshipRequests).ToArray();
        result.Map = CreateMap(width, height, result.Battleships).ToArray();
        return result;
    }
}