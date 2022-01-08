namespace Nomad.Sorter.Domain.ValueObjects;

/// <summary>
/// Current docking information for a bay.
/// </summary>
/// <param name="VehicleRegistration">The registration of the vehicle that is docked in the bay.</param>
/// <param name="DockedAtUtc">The UTC time the vehicle docked.</param>
/// <param name="DepartingUtc">The UTC time the vehicle aims to depart.</param>
/// <param name="ParcelCapacity">The amount of parcel's the vehicle can drop off.</param>
/// <param name="DeliveryRegionId">The delivery region ID that the vehicle is heading.</param>
public record DockingInformation(
    string VehicleRegistration,
    DateTime DockedAtUtc,
    DateTime DepartingUtc,
    int ParcelCapacity,
    string DeliveryRegionId
);