using Convey.CQRS.Events;

namespace Nomad.Sorter.Application.Events.Inbound;

public record ParcelLoadedEvent(
    string ParcelId,
    string BayId
) : IEvent;