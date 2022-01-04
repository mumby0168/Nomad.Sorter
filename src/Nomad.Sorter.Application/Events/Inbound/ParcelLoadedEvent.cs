using Nomad.Sorter.Application.Abstractions;

namespace Nomad.Sorter.Application.Events.Inbound;

public record ParcelLoadedEvent(
    string ParcelId,
    string BayId
) : IEvent;