using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nomad.Sorter.Infrastructure.Data;
using Nomad.Sorter.Infrastructure.Messaging;

namespace Nomad.Sorter.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNomadSorterInfrastructure(this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment hostEnvironment)
    {
        services.AddMessaging(configuration, hostEnvironment);
        services.AddCosmos(hostEnvironment);
        return services;
    }
}