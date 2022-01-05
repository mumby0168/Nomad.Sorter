using Nomad.Abstractions.Cqrs;

namespace Nomad.Sorter.Application.Events.Inbound;

public record ParcelLoadedEvent(
    string ParcelId,
    string BayId
) : IEvent;