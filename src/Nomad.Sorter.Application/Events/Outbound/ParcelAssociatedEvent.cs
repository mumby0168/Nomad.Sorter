using Convey.CQRS.Events;

namespace Nomad.Sorter.Application.Events.Outbound;

public record ParcelAssociatedEvent(
    string ParcelId,
    string VehicleRegistration,
    string BayId
) : IEvent;