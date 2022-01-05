using Nomad.Sorter.Application.Abstractions;

namespace Nomad.Sorter.Application.Events.Inbound;

public record VehicleDockedEvent(
    string VehicleRegistration,
    string DeliveryRegionId,
    string BayId,
    int ParcelCapacity,
    DateTime DepartingUtc,
    DateTime DockedAtUtc
) : IEvent;