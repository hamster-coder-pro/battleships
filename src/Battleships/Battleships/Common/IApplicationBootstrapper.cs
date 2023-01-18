namespace Battleships.Console;

public interface IApplicationBootstrapper
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}