using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Nomad.Sorter.Application.Events.Inbound;

namespace Nomad.Sorter.Infrastructure.Messaging.Consumers;

public class ParcelInductedEventConsumer : IConsumer<ParcelInductedEvent>
{
    private readonly ILogger<ParcelInductedEventConsumer> _logger;
    private readonly IMediator _mediator;

    public ParcelInductedEventConsumer(ILogger<ParcelInductedEventConsumer> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ParcelInductedEvent> context)
    {
        try
        {
            _logger.LogInformation(
                "Processing parcel inducted event for parcel with ID {ParcelId} and client ID {ClientId}",
                context.Message.ParcelId, context.Message.ClientId);

            await _mediator.Publish(context.Message, context.CancellationToken);

            _logger.LogInformation(
                "Successfully processed parcel inducted event for parcel with ID {ParcelId} and client ID {ClientId}",
                context.Message.ParcelId, context.Message.ClientId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to process parcel inducted event for parcel with ID {ParcelId} and client ID {ClientId}",
                context.Message.ParcelId, context.Message.ClientId);

            throw;
        }
    }
}