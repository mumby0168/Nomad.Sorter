using Newtonsoft.Json;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Domain.Enums;
using Nomad.Sorter.Domain.Identitifiers;
using Nomad.Sorter.Domain.ValueObjects;

namespace Nomad.Sorter.Domain.Entities;

/// <inheritdoc cref="IParcel"/>
public class Parcel : BaseEntity, IParcel
{
    /// <inheritdoc cref="IParcel"/>
    [JsonIgnore]
    public ParcelId ParcelId { get; }

    /// <inheritdoc cref="IParcel"/>
    public ParcelStatus Status { get; }
    
    /// <inheritdoc cref="IParcel"/>
    public DeliveryInformation DeliveryInformation { get; }

    /// <summary>
    /// Creates an instance of a parcel.
    /// </summary>
    /// <param name="parcelId">The <see cref="ParcelId"/></param>
    /// <param name="deliveryInformation">The<see cref="DeliveryInformation"/> about the parcel</param>
    /// <remarks>This creates a parcel with the <see cref="ParcelStatus"/> PreAdvice</remarks>
    internal Parcel(ParcelId parcelId, DeliveryInformation deliveryInformation) : base(deliveryInformation.RegionId)
    {
        Id = parcelId;
        ParcelId = parcelId;
        Status = ParcelStatus.PreAdvice;
        DeliveryInformation = deliveryInformation;
    }

    /// <summary>
    /// Creates an instance of a parcel been deserialized from JSON.
    /// </summary>
    /// <param name="id">The ID of the parcel</param>
    /// <param name="status">The <see cref="ParcelStatus"/> of the parcel.</param>
    /// <param name="deliveryInformation">The <see cref="DeliveryInformation"/> about the parcel</param>
    [JsonConstructor]
    private Parcel(
        string id,
        ParcelStatus status,
        DeliveryInformation deliveryInformation) : base(deliveryInformation.RegionId)
    {
        Id = id;
        ParcelId = id.ToParcelId();
        Status = status;
        DeliveryInformation = deliveryInformation;
    }
}