namespace Battleships.Console;

public class GameInfo
{
    public GameInfo()
    {
        Map = new List<SquareInfo>();
        Battleships = new List<Battleship>();
    }

    public int Shots { get; set; }

    public int Hits { get; set; }

    public int Misses { get; set; }

    public int Sinks { get; set; }

    public int MapWidth
    {
        get; private set;
    }

    public int MapHeight
    {
        get; private set;
    }

    private IList<SquareInfo> _map;

    public IEnumerable<SquareInfo> Map
    {
        get
        {
            return _map;
        }
        set
        {
            _map = value.AsList();
            MapWidth = _map.Max(x => (int?)x.Column) ?? 0;
            MapHeight = _map.Max(x => (int?)x.Row) ?? 0;
        }
    }

    public ICollection<Battleship> Battleships { get; set; }

    public bool InProgress { get; set; }
}