using Convey.CQRS.Events;
using Nomad.Sorter.Application.Events.Inbound;

namespace Nomad.Sorter.Application.Handlers.Events;

public class ParcelLoadedEventHandler : IEventHandler<ParcelLoadedEvent>
{
    public Task HandleAsync(ParcelLoadedEvent parcelLoadedEvent, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }   
}