using Nomad.Sorter.Domain.Enums;
using Nomad.Sorter.Domain.Identitifiers;
using Nomad.Sorter.Domain.ValueObjects;

namespace Nomad.Sorter.Domain.Entities.Abstractions;

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
    /// The region in which the parcel will be delivered.
    /// </summary>
    string DeliveryRegionId { get; }

    /// <summary>
    /// The delivery information for the parcel.
    /// </summary>
    DeliveryInformation DeliveryInformation { get; }
}