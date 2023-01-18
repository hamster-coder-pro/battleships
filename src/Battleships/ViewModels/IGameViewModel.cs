namespace Battleships.Console;

public interface IGameViewModel
{
    /// <summary>
    /// Start UI :)
    /// </summary>
    void Run();

    /// <summary>
    /// Repaint view
    /// </summary>
    void Repaint();

    /// <summary>
    /// Start/Restart game
    /// </summary>
    void Restart();

    /// <summary>
    /// End current game
    /// </summary>
    void Terminate();

    /// <summary>
    /// Shoot!
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    void Shoot(int column, int row);
}