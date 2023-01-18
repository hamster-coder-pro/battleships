
namespace Battleships.Console;

public interface IGameDesigner
{
    GameInfo GenerateMap(
        int width,
        int height,
        IEnumerable<BattleshipRequest> battleshipRequests
    );
}