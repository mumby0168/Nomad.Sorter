using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Nomad.Sorter.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNomadSorterApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(ServiceCollectionExtensions));
        return services;
    }
}