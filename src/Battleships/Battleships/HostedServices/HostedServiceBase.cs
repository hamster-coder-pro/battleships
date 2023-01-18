using Microsoft.Extensions.Hosting;

namespace Battleships.Console;

public abstract class HostedServiceBase : IHostedService
{
    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}