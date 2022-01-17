using Nomad.Abstractions.Cqrs;

namespace Nomad.Sorter.Application.Events.Inbound;

public record VehicleDockedEvent(
    string VehicleRegistration,
    string DeliveryRegionId,
    string BayId,
    int ParcelCapacity,
    DateTime DepartingAtUtc,
    DateTime DockedAtUtc
) : IEvent;