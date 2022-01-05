using Newtonsoft.Json;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Domain.Enums;
using Nomad.Sorter.Domain.Identitifiers;
using Nomad.Sorter.Domain.ValueObjects;

namespace Nomad.Sorter.Domain.Entities;

public class Parcel : IParcel
{
    [JsonIgnore]
    public ParcelId ParcelId { get; }

    public ParcelStatus Status { get; }
    public string DeliveryRegionId { get; }
    public DeliveryInformation DeliveryInformation { get; }
}