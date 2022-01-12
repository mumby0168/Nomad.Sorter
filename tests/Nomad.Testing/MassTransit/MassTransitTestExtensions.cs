using MassTransit;
using MassTransit.AspNetCoreIntegration;
using MassTransit.AspNetCoreIntegration.HealthChecks;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nomad.Sorter.Infrastructure.Messaging.Consumers;
using Nomad.Testing.Extensions;

namespace Nomad.Testing.MassTransit;

public static class MassTransitTestExtensions
{
    public static IServiceCollection AddMassTransitTestServices(this IServiceCollection services)
    {
        services.AddSingleton<MassTransitConsumerInvoker>();
        services.RemoveNamespaceServices("MassTransit");

        services.AddMassTransit(c => c.UsingInMemory());

        return services;
    }
}