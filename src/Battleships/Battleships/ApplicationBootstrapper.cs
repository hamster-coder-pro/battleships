namespace Battleships.Console;

internal class ApplicationBootstrapper : IApplicationBootstrapper
{
    private IGameViewModel GameViewModel { get; }

    public ApplicationBootstrapper(IGameViewModel gameViewModel)
    {
        GameViewModel = gameViewModel;
    }

    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        GameViewModel.Restart();
        GameViewModel.Run();
        return Task.CompletedTask;
    }
}