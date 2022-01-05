using Nomad.Abstractions.Cqrs;

namespace Nomad.Sorter.Application.Events.Inbound;

public record ParcelInductedEvent(
    string ParcelId,
    string ClientId
) : IEvent;