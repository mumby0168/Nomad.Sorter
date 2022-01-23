using Convey.CQRS.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Nomad.Sorter.Application.Events.Inbound;

namespace Nomad.Sorter.Infrastructure.Messaging.Consumers;

public class ParcelLoadedEventConsumer : IConsumer<ParcelLoadedEvent>
{
    private readonly ILogger<ParcelLoadedEventConsumer> _logger;
    private readonly IEventDispatcher _eventDispatcher;

    public ParcelLoadedEventConsumer(ILogger<ParcelLoadedEventConsumer> logger, IEventDispatcher eventDispatcher)
    {
        _logger = logger;
        _eventDispatcher = eventDispatcher;
    }

    public async Task Consume(ConsumeContext<ParcelLoadedEvent> context)
    {
        try
        {
            _logger.LogInformation(
                "Processing parcel loaded event for parcel with ID {ParcelId}",
                context.Message.ParcelId);

            await _eventDispatcher.PublishAsync(context.Message, context.CancellationToken);

            _logger.LogInformation(
                "Successfully processed parcel loaded event for parcel with ID {ParcelId}",
                context.Message.ParcelId);      
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Failed to process parcel loaded event for parcel with ID {ParcelId}",
                context.Message.ParcelId);

            throw;
        }
    }
}