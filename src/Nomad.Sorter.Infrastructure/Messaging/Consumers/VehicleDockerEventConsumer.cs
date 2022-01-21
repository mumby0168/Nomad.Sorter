using Convey.CQRS.Events;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.Extensions.Logging;
using Nomad.Sorter.Application.Events.Inbound;

namespace Nomad.Sorter.Infrastructure.Messaging.Consumers;

public class VehicleDockerEventConsumer : IConsumer<VehicleDockedEvent>
{
    private readonly ILogger<VehicleDockerEventConsumer> _logger;
    private readonly IEventDispatcher _eventDispatcher;

    public VehicleDockerEventConsumer(ILogger<VehicleDockerEventConsumer> logger, IEventDispatcher eventDispatcher)
    {
        _logger = logger;
        _eventDispatcher = eventDispatcher;
    }

    public async Task Consume(ConsumeContext<VehicleDockedEvent> context)
    {
        try
        {
            _logger.LogDebug(
                "Processing vehicle docked event for vehicle {VehicleRegistration}",
                context.Message.VehicleRegistration);

            await _eventDispatcher.PublishAsync(context.Message, context.CancellationToken);

            _logger.LogDebug(
                "Processing vehicle docked event for vehicle {VehicleRegistration}",
                context.Message.VehicleRegistration);
        }
        catch (Exception e)
        {
            _logger.LogError(
                e, "Failed Processing vehicle docked event for vehicle {VehicleRegistration}",
                context.Message.VehicleRegistration);

            throw;
        }
    }
}