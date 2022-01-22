using Convey.CQRS.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Nomad.Sorter.Application.Events.Inbound;

namespace Nomad.Sorter.Infrastructure.Messaging.Consumers;

public class ParcelInductedEventConsumer : IConsumer<ParcelInductedEvent>
{
    private readonly ILogger<ParcelInductedEventConsumer> _logger;
    private readonly IEventDispatcher _eventDispatcher;

    public ParcelInductedEventConsumer(ILogger<ParcelInductedEventConsumer> logger, IEventDispatcher eventDispatcher)
    {
        _logger = logger;
        _eventDispatcher = eventDispatcher;
    }

    public async Task Consume(ConsumeContext<ParcelInductedEvent> context)
    {
        try
        {
            _logger.LogInformation(
                "Processing parcel inducted event for parcel with ID {ParcelId}",
                context.Message.ParcelId);

            await _eventDispatcher.PublishAsync(context.Message, context.CancellationToken);

            _logger.LogInformation(
                "Successfully processed parcel inducted event for parcel with ID {ParcelId}",
                context.Message.ParcelId);      
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Failed to process parcel inducted event for parcel with ID {ParcelId}",
                context.Message.ParcelId);

            throw;
        }
    }
}