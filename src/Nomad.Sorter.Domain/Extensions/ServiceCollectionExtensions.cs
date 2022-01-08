using Microsoft.Extensions.DependencyInjection;
using Nomad.Sorter.Domain.Factories;

namespace Nomad.Sorter.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNomadSorterDomain(this IServiceCollection services)
    {
        services.AddSingleton<IParcelFactory, ParcelFactory>();
        return services;
    }
}