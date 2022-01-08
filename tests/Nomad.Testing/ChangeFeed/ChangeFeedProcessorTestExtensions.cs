using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nomad.Sorter.Infrastructure.Cosmos;

namespace Nomad.Testing.ChangeFeed;

public static class ChangeFeedProcessorTestExtensions
{
    public static IServiceCollection AddChangeFeedProcessorTestInvokers(this IServiceCollection services)
    {
        services.RemoveAll(typeof(ChangeFeedHostedService));
        return services.AddSingleton(typeof(ChangeFeedProcessorInvoker<>));
    }
}