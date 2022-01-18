using MassTransit;
using Microsoft.Extensions.Logging;
using Nomad.Sorter.Application.Events.Outbound;
using Nomad.Sorter.Application.Infrastructure;

namespace Nomad.Sorter.Infrastructure.Messaging.Services;

public class MassTransitDispatcher : IDispatcher
{
    private readonly ILogger<MassTransitDispatcher> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitDispatcher(
        ILogger<MassTransitDispatcher> logger,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async ValueTask DispatchParcelAssociatedEvents(IEnumerable<ParcelAssociatedEvent> parcelAssociatedEvents)
    {
        var associatedEvents = parcelAssociatedEvents.ToList();
        
        _logger.LogInformation("Dispatching {ParcelAssociatedEventsCount} parcel associated event", 
            associatedEvents.Count);

        await _publishEndpoint.Publish(associatedEvents);
        
        _logger.LogInformation("Successfully dispatched {ParcelAssociatedEventsCount} parcel associated event", 
            associatedEvents.Count);
    }
}