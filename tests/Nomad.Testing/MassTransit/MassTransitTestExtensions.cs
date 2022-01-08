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
        services.RemoveAll(typeof(MassTransitHostedService));
        services.AddSingleton<MassTransitConsumerInvoker>();

        // services.RemoveAll(typeof(IBus));
        // services.RemoveAll(typeof(IBusControl));
        // services.RemoveAll(typeof(BusHealthCheck));
        //
        // services.AddMassTransitInMemoryTestHarness(cfg =>
        // {
        //     cfg.AddConsumer<ParcelPreAdviceCommandConsumer>();
        //     cfg.AddConsumerTestHarness<ParcelPreAdviceCommandConsumer>();
        // });

        return services;
    }
}