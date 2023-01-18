using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Battleships.Console;

public sealed class RunApplicationHostedService<TApplication> : RunAfterStartupHostedServiceBase
    where TApplication: IApplicationBootstrapper
{
    private IServiceProvider ServiceProvider { get; }

    public RunApplicationHostedService(IHostApplicationLifetime hostApplicationLifetime, IServiceProvider serviceProvider) : base(hostApplicationLifetime)
    {
        ServiceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        var application = scope.ServiceProvider.GetRequiredService<TApplication>();
        await application.ExecuteAsync(cancellationToken);
    }
}