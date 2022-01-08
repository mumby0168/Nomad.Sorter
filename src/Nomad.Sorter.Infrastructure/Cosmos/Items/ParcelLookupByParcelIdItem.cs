using Microsoft.Azure.CosmosRepository;
using Newtonsoft.Json;
using Nomad.Sorter.Domain.Identitifiers;

namespace Nomad.Sorter.Infrastructure.Cosmos.Items;

public class ParcelLookupByParcelIdItem : FullItem
{
    public ParcelLookupByParcelIdItem(string id, string deliveryRegionId)
    {
        Id = id;
        PartitionKey = id;
        DeliveryRegionId = deliveryRegionId;
    }
    
    [JsonProperty("pk")]
    public string PartitionKey { get; set; }

    public string DeliveryRegionId { get; set; }
    
    protected override string GetPartitionKeyValue() => PartitionKey;
}