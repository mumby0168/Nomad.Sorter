using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nomad.Sorter.Application.Events.Outbound;
using Nomad.Sorter.Application.Infrastructure;
using Nomad.Sorter.Infrastructure.Extensions;
using Nomad.Sorter.Infrastructure.Messaging.Consumers;
using Nomad.Sorter.Infrastructure.Messaging.Services;

namespace Nomad.Sorter.Infrastructure.Messaging;

public static class MessagingExtensions
{
    internal static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment hostEnvironment)
    {
        services.AddScoped<IDispatcher, MassTransitDispatcher>();
        services.AddMassTransit(massTransit =>
        {
            massTransit.AddConsumer<ParcelPreAdviceCommandConsumer>();
            massTransit.AddConsumer<ParcelInductedEventConsumer>();
            massTransit.AddConsumer<VehicleDockerEventConsumer>();

            massTransit.UsingAzureServiceBus((registration, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("ServiceBus"));
                cfg.ExcludeInterfacesFromTopology();
                cfg.ConfigureQueues(registration);
                cfg.ConfigureTopicSubscriptions(registration);
                cfg.ConfigurePublishers();
            });
        });

        if (hostEnvironment.IsNotFunctionalTests())
        {
            services.AddMassTransitHostedService();   
        }

        return services;
    }

    public static void ExcludeInterfacesFromTopology(this IServiceBusBusFactoryConfigurator cfg)
    {
        cfg.Publish<IEvent>(configurator => configurator.Exclude = true);
        cfg.Publish<ICommand>(configurator => configurator.Exclude = true);
    }

    private static void ConfigurePublishers(this IServiceBusBusFactoryConfigurator cfg)
    {
        cfg.Message<ParcelAssociatedEvent>(x =>
            x.SetEntityName(ServiceBusConstants.Topics.ParcelAssociatedTopic));
    }


    private static void ConfigureTopicSubscriptions(
        this IServiceBusBusFactoryConfigurator cfg,
        IRegistration registration)
    {
        cfg.SubscriptionEndpoint(
            ServiceBusConstants.AppName,
            ServiceBusConstants.Topics.ParcelInductedTopic,
            configurator =>
            {
                configurator.PublishFaults = false;
                configurator.ConfigureConsumeTopology = false;
                configurator.ConfigureConsumer<ParcelInductedEventConsumer>(registration);
            });

        cfg.SubscriptionEndpoint(
            ServiceBusConstants.AppName,
            ServiceBusConstants.Topics.VehicleDockedTopic,
            configurator =>
            {
                configurator.PublishFaults = false;
                configurator.ConfigureConsumeTopology = false;
                configurator.ConfigureConsumer<VehicleDockerEventConsumer>(registration);
            });
        
        cfg.SubscriptionEndpoint(
            ServiceBusConstants.AppName,
            ServiceBusConstants.Topics.ParcelLoadedTopic,
            configurator =>
            {
                configurator.PublishFaults = false;
                configurator.ConfigureConsumeTopology = false;
                configurator.ConfigureConsumer<ParcelLoadedEventConsumer>(registration);
            });
    }
    
    private static void ConfigureQueues(
        this IServiceBusBusFactoryConfigurator cfg, 
        IRegistration registration)
    {
        cfg.ReceiveEndpoint(
            ServiceBusConstants.Queues.ParcelPreAdviceQueue,
            configurator =>
            {
                configurator.ConfigureConsumer<ParcelPreAdviceCommandConsumer>(registration);
                configurator.ConfigureConsumeTopology = false;
                configurator.PublishFaults = false;
            });
    }
}