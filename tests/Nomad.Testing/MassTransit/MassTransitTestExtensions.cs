using MassTransit;
using MassTransit.AspNetCoreIntegration;
using MassTransit.AspNetCoreIntegration.HealthChecks;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nomad.Sorter.Infrastructure.Messaging.Consumers;

namespace Nomad.Testing.MassTransit;

public static class MassTransitTestExtensions
{
    public static IServiceCollection AddMassTransitTestServices(this IServiceCollection services)
    {
        services.AddSingleton<MassTransitConsumerInvoker>();
        services.RemoveAll(typeof(MassTransitHostedService));
       
        
        var descriptors = services
            .Where(d =>
                d.ServiceType.Namespace is not null &&
                d.ServiceType.Namespace.StartsWith("MassTransit", StringComparison.OrdinalIgnoreCase))
            .ToList();

        foreach (var d in descriptors)
        {
            services.Remove(d);
        }

        services.AddMassTransit(c => c.UsingInMemory());

        return services;
    }
}