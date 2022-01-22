using Microsoft.Azure.Cosmos;

namespace Cosmology;

public static class CosmologyClient
{
    private static CosmosClient? _client;

    public static CosmosClient CosmosDbClient
    {
        get
        {
            if (_client is not null) 
                return _client;
            
            try
            {
                _client = new CosmosClient(Environment.GetEnvironmentVariable(Constants.ConnectionStringEnvironmentVariable));
            }
            catch
            {
                throw new Exception($"Please make sure a cosmos connection string via the env var {Constants.ConnectionStringEnvironmentVariable}");
            }

            return _client;
        }
    }
}