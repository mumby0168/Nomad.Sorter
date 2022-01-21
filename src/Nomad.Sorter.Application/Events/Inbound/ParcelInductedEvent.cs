using Convey.CQRS.Events;

namespace Nomad.Sorter.Application.Events.Inbound;

public record ParcelInductedEvent(
    string ParcelId
) : IEvent;