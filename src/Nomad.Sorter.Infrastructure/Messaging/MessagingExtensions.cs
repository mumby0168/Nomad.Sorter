using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomad.Sorter.Application.Commands;
using Nomad.Sorter.Infrastructure.Messaging.Consumers;

namespace Nomad.Sorter.Infrastructure.Messaging;

internal static class MessagingExtensions
{
    internal static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(massTransit =>
        {
            massTransit.AddConsumer<ParcelPreAdviceCommandConsumer>();

            massTransit.UsingAzureServiceBus((registration, serviceBusConfig) =>
            {
                serviceBusConfig.Host(configuration.GetConnectionString("ServiceBus"));
                serviceBusConfig.ConfigureConsumerForQueue<ParcelPreAdviceCommandConsumer>(ServiceBusConstants.Queues.ParcelPreAdviceQueue, registration);
            });
        });

        services.AddMassTransitHostedService();

        return services;
    }

    private static void ConfigureConsumerForQueue<T>(this IServiceBusBusFactoryConfigurator configurator, string queueName,
        IBusRegistrationContext registrationContext) where T : IConsumer =>
        configurator.ReceiveEndpoint(queueName,
            x => x.ConfigureConsumer<ParcelPreAdviceCommandConsumer>(registrationContext));
}