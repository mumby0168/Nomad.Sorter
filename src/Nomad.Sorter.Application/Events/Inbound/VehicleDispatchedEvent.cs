using Nomad.Sorter.Application.Abstractions;

namespace Nomad.Sorter.Application.Events.Inbound;

public record VehicleDispatchedEvent(
    string VehicleRegistration,
    string DeliveryRegionId,
    string BayId
) : IEvent;