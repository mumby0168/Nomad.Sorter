using Microsoft.Azure.CosmosRepository;
using Newtonsoft.Json;
using Nomad.Sorter.Domain.Identitifiers;

namespace Nomad.Sorter.Infrastructure.Cosmos.Items;

public class ParcelIdLookup : FullItem
{
    [JsonProperty("pk")]
    public string PartitionKey { get; set; }

    protected override string GetPartitionKeyValue() => PartitionKey;

    public ParcelIdLookup(ParcelId parcelId)
    {
        Id = parcelId;
        PartitionKey = parcelId;
    }
}