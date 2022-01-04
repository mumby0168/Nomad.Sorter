using Nomad.Sorter.Application.Abstractions;

namespace Nomad.Sorter.Application.Events.Inbound;

public record ParcelInductedEvent(
    string ParcelId,
    string ClientId
) : IEvent;