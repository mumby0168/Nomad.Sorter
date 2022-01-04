using Nomad.Sorter.Application.Abstractions;

namespace Nomad.Sorter.Application.Events.Outbound;

public record ParcelAssociatedEvent(
    string ParcelId,
    string VehicleRegistration
) : IEvent;