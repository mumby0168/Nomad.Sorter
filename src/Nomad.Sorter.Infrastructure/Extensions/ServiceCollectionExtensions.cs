using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomad.Sorter.Infrastructure.Cosmos;
using Nomad.Sorter.Infrastructure.Messaging;

namespace Nomad.Sorter.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNomadSorterInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMessaging(configuration);
        services.AddCosmos();
        return services;
    }
}