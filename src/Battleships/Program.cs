using Battleships.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();

await host.RunAsync();

static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    services.Add(new ServiceDescriptor(typeof(IApplicationBootstrapper), typeof(ApplicationBootstrapper), ServiceLifetime.Scoped));
    services.AddHostedService<RunApplicationHostedService<IApplicationBootstrapper>>();

    services.Add(new ServiceDescriptor(typeof(IGameManager), typeof(GameManager), ServiceLifetime.Singleton));
    services.Add(new ServiceDescriptor(typeof(IGameDesigner), typeof(GameDesigner), ServiceLifetime.Singleton));
    services.Add(new ServiceDescriptor(typeof(IGameViewModel), typeof(GameViewModel), ServiceLifetime.Transient));
}