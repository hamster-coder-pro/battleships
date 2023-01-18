namespace Battleships.Console;

public class SquareInfo
{
    public int Row { get; set; }

    public int Column { get; set; }

    public bool IsBusy { get; set; }

    public bool IsShown { get; set; }

    public Battleship? Battleship { get; set; }

    public bool IsNear(SquareInfo to)
    {
        return IsSame(to) == false
               && to.Row >= Row - 1 
               && to.Row <= Row + 1
               && to.Column >= Column - 1 
               && to.Column <= Column + 1
            ;
    }

    private bool IsSame(SquareInfo to)
    {
        return to.Row == Row && to.Column == Column;
    }

    public override string ToString()
    {
        return $"{Row} {Column}";
    }
}