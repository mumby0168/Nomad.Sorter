using MassTransit;
using MassTransit.Mediator;
using Microsoft.Extensions.Logging;
using Nomad.Sorter.Application.Events.Inbound;

namespace Nomad.Sorter.Infrastructure.Messaging.Consumers;

public class VehicleDockerEventConsumer : IConsumer<VehicleDockedEvent>
{
    private readonly ILogger<VehicleDockerEventConsumer> _logger;
    private readonly IMediator _mediator;

    public VehicleDockerEventConsumer(ILogger<VehicleDockerEventConsumer> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<VehicleDockedEvent> context)
    {
        try
        {
            _logger.LogDebug(
                "Processing vehicle docked event for vehicle {VehicleRegistration}",
                context.Message.VehicleRegistration);

            await _mediator.Publish(context.Message, context.CancellationToken);

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