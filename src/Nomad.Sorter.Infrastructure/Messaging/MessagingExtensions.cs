using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using MassTransit.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomad.Abstractions.Cqrs;
using Nomad.Sorter.Application.Commands;
using Nomad.Sorter.Application.Events.Inbound;
using Nomad.Sorter.Infrastructure.Messaging.Consumers;

namespace Nomad.Sorter.Infrastructure.Messaging;

internal static class MessagingExtensions
{
    internal static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(massTransit =>
        {
            massTransit.AddConsumer<ParcelPreAdviceCommandConsumer>();
            massTransit.AddConsumer<ParcelInductedEventConsumer>();

            massTransit.UsingAzureServiceBus((registration, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("ServiceBus"));
                
                //Configures a command to be consumed from a queue with the given name.
                cfg.ReceiveEndpoint(
                    queueName: ServiceBusConstants.Queues.ParcelPreAdviceQueue,
                    e =>
                    {
                        e.ConfigureConsumer<ParcelPreAdviceCommandConsumer>(registration);
                        e.ConfigureConsumeTopology = false;
                    }
                );

                //Configures a subscription with the given name on a topic with the given name.
                cfg.SubscriptionEndpoint(
                    subscriptionName: ServiceBusConstants.AppName,
                    topicPath: ServiceBusConstants.Topics.ParcelInductedTopic,
                    e => e.ConfigureConsumer<ParcelInductedEventConsumer>(registration)
                );
            });
        });

        services.AddMassTransitHostedService();

        return services;
    }
}