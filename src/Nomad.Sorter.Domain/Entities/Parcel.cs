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

    internal Parcel(ParcelId parcelId, DeliveryInformation deliveryInformation) : base(deliveryInformation.RegionId)
    {
        Id = parcelId;
        ParcelId = parcelId;
        Status = ParcelStatus.PreAdvice;
        DeliveryInformation = deliveryInformation;
    }

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