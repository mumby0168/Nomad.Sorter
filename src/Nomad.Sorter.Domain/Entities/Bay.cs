using Microsoft.Azure.CosmosRepository;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Domain.Enums;
using Nomad.Sorter.Domain.Identitifiers;

namespace Nomad.Sorter.Domain.Entities;

public class Bay : BaseEntity, IBay
{
    [JsonIgnore] 
    public BayId BayId { get; }

    [JsonConverter(typeof(StringEnumConverter))]
    public BayStatus Status { get; }

    public Bay(BayId bayId, BayStatus status) : base(nameof(Bay))
    {
        BayId = bayId;
        Status = status;
        Id = bayId;
    }

    [JsonConstructor]
    private Bay(
        string id,
        BayStatus status) : base(nameof(Bay))
    {
        Status = status;
        BayId = id.ToBayId();
        Id = id;
    }
}