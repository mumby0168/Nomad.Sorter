using Nomad.Sorter.Domain.Enums;
using Nomad.Sorter.Domain.Identitifiers;
using Nomad.Sorter.Domain.ValueObjects;

namespace Nomad.Sorter.Domain.Entities.Abstractions;

public interface IBay
{
    /// <summary>
    /// The ID of the bay.
    /// </summary>
    BayId BayId { get; }
    
    /// <summary>
    /// The status of the bay.
    /// </summary>
    BayStatus Status { get; }
    
    /// <summary>
    /// The time the bay was last updated.
    /// </summary>
    DateTime LastUpdatedTimeUtc { get; }
    
    /// <summary>
    /// The time the bay was created.
    /// </summary>
    DateTime? CreatedTimeUtc { get; }
    
    /// <summary>
    /// The information about a docked vehicle in a bay.
    /// </summary>
    /// <remarks>null if there is not vehicle docked in the bay.</remarks>
    DockingInformation? DockingInformation { get; }

    /// <summary>
    /// Docks a vehicle into a <see cref="IBay"/>
    /// </summary>
    /// <param name="vehicleRegistration">The registration of a the vehicle docking.</param>
    /// <param name="dockedAtUtc">The time the vehicle docked.</param>
    /// <param name="departingUtc">The time the vehicle is departing.</param>
    /// <param name="parcelCapacity">The amount of parcels the vehicle can deliver.</param>
    /// <param name="deliveryRegionId">The delivery region ID the vehicle is delivering to.</param>
    void DockVehicle(
        string vehicleRegistration,
        DateTime dockedAtUtc,
        DateTime departingUtc,
        int parcelCapacity,
        string deliveryRegionId);
}