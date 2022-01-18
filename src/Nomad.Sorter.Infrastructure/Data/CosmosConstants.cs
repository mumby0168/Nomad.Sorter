namespace Nomad.Sorter.Infrastructure.Data;

public static class CosmosConstants
{
    public const string DatabaseName = "nomad-sorter-db";

    public const string DefaultPartitionKey = "/pk";
    
    public static class Containers
    {
        public const string Parcels = "parcels";
        
        public const string Bays = "bays";
    }
}   