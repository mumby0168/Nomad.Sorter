using Newtonsoft.Json;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Domain.Identitifiers;

namespace Nomad.Sorter.Domain.Entities;

public class Parcel : IParcel
{
    [JsonIgnore]
    public ParcelId ParcelId { get; }
    
}