using Microsoft.Azure.CosmosRepository;
using Newtonsoft.Json;

namespace Nomad.Sorter.Domain.Entities.Abstractions;

public abstract class BaseEntity : FullItem
{
    protected override string GetPartitionKeyValue() => PartitionKey;

    [JsonProperty("pk")] public string PartitionKey { get; set; } = default!;

    protected BaseEntity(string? partitionKey = null) => 
        PartitionKey = partitionKey ?? Id;
}