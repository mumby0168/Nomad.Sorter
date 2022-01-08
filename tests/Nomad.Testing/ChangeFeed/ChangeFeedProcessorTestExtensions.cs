using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nomad.Sorter.Infrastructure.Cosmos.ChangeFeed;

namespace Nomad.Testing.ChangeFeed;

public static class ChangeFeedProcessorTestExtensions
{
    public static IServiceCollection AddChangeFeedProcessorTestInvokers(this IServiceCollection services)
    {
        services.RemoveAll(typeof(ChangeFeedProcessorService));
        return services.AddSingleton(typeof(ChangeFeedProcessorInvoker<>));
    }
}