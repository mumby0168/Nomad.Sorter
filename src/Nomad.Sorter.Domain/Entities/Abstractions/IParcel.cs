using Nomad.Sorter.Domain.Enums;
using Nomad.Sorter.Domain.Identitifiers;
using Nomad.Sorter.Domain.ValueObjects;

namespace Nomad.Sorter.Domain.Entities.Abstractions;

/// <summary>
/// Represents a parcel placed into the sorter.
/// </summary>
public interface IParcel
{
    /// <summary>
    /// The id of the parcel.
    /// </summary>
    ParcelId ParcelId { get; }
    
    /// <summary>
    /// The status of the parcel.
    /// </summary>
    ParcelStatus Status { get; }

    /// <summary>
    /// The delivery information for the parcel.
    /// </summary>
    DeliveryInformation DeliveryInformation { get; }
}