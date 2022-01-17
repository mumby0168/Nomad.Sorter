using Nomad.Sorter.Application.Events.Outbound;

namespace Nomad.Sorter.Application.Infrastructure;

public interface IDispatcher
{
    ValueTask DispatchParcelAssociatedEvents(IEnumerable<ParcelAssociatedEvent> parcelAssociatedEvents);
}