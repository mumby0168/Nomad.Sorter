using Nomad.Abstractions.Cqrs;

namespace Nomad.Sorter.Application.Events.Outbound;

public record ParcelAssociatedEvent(
    string ParcelId,
    string VehicleRegistration
) : IEvent;