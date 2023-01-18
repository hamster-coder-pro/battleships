using Spectre.Console;
using Spectre.Console.Rendering;

namespace Battleships.Console;

class GameViewModel : IGameViewModel
{
    private IGameManager GameManager { get; }

    private IGameDesigner GameDesigner { get; }

    private GameInfo GameInfo { get; set; }

    public bool EasyMode { get; set; }

    public GameViewModel(IGameManager gameManager, IGameDesigner gameDesigner)
    {
        EasyMode = true;
        GameInfo = new GameInfo();

        GameManager = gameManager;
        GameDesigner = gameDesigner;
    }

    private IRenderable CreateBattlefield()
    {
        IRenderable RenderSquareInfo(SquareInfo source)
        {
            if (source.Battleship != null)
            {
                if (source.IsShown)
                {
                    return new Markup("X");
                }

                if (EasyMode)
                {
                    return new Markup("o");
                }

                return new Markup("[gray].[/]");

            }
            if (source.IsShown)
            {
                return new Markup("*");
            }
            return new Markup("[gray].[/]");
        }

        Grid battlefieldGrid = new Grid();
        for (int col = 1; col <= GameInfo.MapWidth; col++)
        {
            battlefieldGrid.AddColumn(new GridColumn().NoWrap().Centered());
        }

        for (int row = 1; row <= GameInfo.MapHeight; row++)
        {
            battlefieldGrid.AddRow(GameInfo.Map.Where(x => x.Row == row).OrderBy(x => x.Column).Select(RenderSquareInfo).ToArray());
        }

        // create column headers grid
        var columnHeaders = new Grid();
        for (int col = 1; col <= GameInfo.MapWidth; col++)
        {
            columnHeaders.AddColumn(new GridColumn().NoWrap().Centered());
        }
        columnHeaders.AddRow(Enumerable.Range(1, GameInfo.MapWidth).Select(x => (IRenderable)new Markup($"{x}")).ToArray());

        // create row headers grid
        var rowHeaders = new Grid();
        rowHeaders.AddColumn(new GridColumn().NoWrap().Centered());
        for (int row = 1; row <= GameInfo.MapHeight; row++)
        {
            rowHeaders.AddRow(new Markup($"{Convert.ToChar(A_CHAR_BYTE + row - 1)}"));
        }

        var result = new Grid();
        result.AddColumn();
        result.AddColumn();
        result.AddRow(new Text(string.Empty), columnHeaders);
        result.AddRow(rowHeaders, battlefieldGrid);

        return result;
    }

    public void Repaint()
    {
        AnsiConsole.Clear();

        var battlefield = CreateBattlefield();
        AnsiConsole.Write(battlefield);

        var statistics = CreateStatistics();

        AnsiConsole.WriteLine();
        AnsiConsole.Write(statistics);

        var hints = CreateHints();
        AnsiConsole.WriteLine();
        AnsiConsole.Write(hints);
    }

    private string RequestInput()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Markup("Input coordinates to fire: ");
        return System.Console.ReadLine() ?? string.Empty;
    }

    private IRenderable CreateStatistics()
    {
        var result = new Grid();

        result.AddColumn(new GridColumn().NoWrap().LeftAligned());
        result.AddColumn(new GridColumn().NoWrap().LeftAligned());
        result.AddRow("Game:", GameInfo.InProgress ? "In progress" : "Ended");
        result.AddRow("Mode:", EasyMode ? "Easy" : "Hard");
        result.AddRow("Shots:", $"{GameInfo.Shots}");
        result.AddRow(new Markup("[green]Hits:[/]"), new Text($"{GameInfo.Hits}"));
        result.AddRow(new Markup("[red]Misses:[/]"), new Text($"{GameInfo.Misses}"));
        result.AddRow(new Markup("[bold blue]Sinks:[/]"), new Text($"{GameInfo.Sinks}"));

        return result;
    }

    private IRenderable CreateHints()
    {
        Grid result = new Grid();
        result.AddColumn();
        result.AddRow(new Markup("Hints:"));
        result.AddRow(new Markup("[gray]Type \"easy\" to easy mode (battleships are visible)[/]"));
        result.AddRow(new Markup("[gray]Type \"hard\" to hard mode (battleships are visible)[/]"));
        result.AddRow(new Markup("[gray]Type \"exit\" to terminate game[/]"));
        if (GameInfo.InProgress)
        {
            result.AddRow(new Markup("[gray]Type \"quit\" to give up[/]"));
            result.AddRow(new Markup("[gray]Type coordinates to shoot (e.g. A10 or J6)[/]"));
        }
        return result;
    }

    // ReSharper disable once InconsistentNaming
    private static readonly byte A_CHAR_BYTE = Convert.ToByte('A');

    public void Run()
    {
        while (true)
        {
            var input = RequestInput();

            try
            {
                if (string.Equals(input, "easy", StringComparison.InvariantCultureIgnoreCase))
                {
                    EasyMode = true;
                    Restart();
                    continue;
                }

                if (string.Equals(input, "hard", StringComparison.InvariantCultureIgnoreCase))
                {
                    EasyMode = false;
                    Restart();
                    continue;
                }

                if (string.Equals(input, "exit", StringComparison.InvariantCultureIgnoreCase))
                {
                    Environment.Exit(0);
                    return;
                }

                if (GameInfo.InProgress == false)
                {
                    throw new Exception("You should start the game");
                }


                if (string.Equals(input, "quit", StringComparison.InvariantCultureIgnoreCase))
                {
                    Terminate();
                    continue;
                }

                if (input.Length == 0 || input.Length > 3)
                {
                    throw new Exception("Invalid coordinates");
                }

                int row = Convert.ToByte(char.ToUpper(input[0])) - A_CHAR_BYTE + 1;

                if (int.TryParse(input[1..], out var column) == false)
                {
                    throw new Exception("Invalid coordinates");
                }

                GameManager.Shoot(GameInfo, row, column);
                Repaint();

                if (GameInfo.Battleships.All(x => x.IsSinked))
                {
                    Terminate();
                }
            }
            catch (Exception exception)
            {
                AnsiConsole.MarkupLine($"[red]{exception.Message}[/]"); ;
            }
        }
    }

    public void Restart()
    {
        GameInfo = GameDesigner.GenerateMap(10, 10, new[]
        {
            new BattleshipRequest() { Size = 5, Count = 1 },
            new BattleshipRequest() { Size = 4, Count = 2 }
        });
        GameInfo.InProgress = true;

        // 
        Repaint();
    }

    public void Terminate()
    {
        GameInfo.InProgress = false;
        GameManager.Reveal(GameInfo.Map);

        // 
        Repaint();
    }

    public void Shoot(int column, int row)
    {
        if (GameInfo.InProgress == false)
        {
            Restart();
        }

        GameManager.Shoot(GameInfo, row, column);
    }
}