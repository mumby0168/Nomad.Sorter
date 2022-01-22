using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Nomad.Sorter.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNomadSorterApplication(this IServiceCollection services)
    {
        services.AddConvey()
            .AddInMemoryCommandDispatcher()
            .AddInMemoryEventDispatcher()
            .AddEventHandlers()
            .AddCommandHandlers();
        return services;
    }
}