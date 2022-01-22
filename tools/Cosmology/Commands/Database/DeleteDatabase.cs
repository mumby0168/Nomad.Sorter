using System.Diagnostics.CodeAnalysis;
using System.Net;
using Cosmology.Settings;
using Microsoft.Azure.Cosmos;
using Spectre.Console;
using Spectre.Console.Cli;
using static Cosmology.CosmologyClient;

namespace Cosmology.Commands.Database;

public class DeleteDatabase : AsyncCommand<DbCommonSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, DbCommonSettings settings)
    {
        try
        {
            var database = CosmosDbClient.GetDatabase(settings.DatabaseName);
            await database.DeleteAsync();
        }
        catch (CosmosException e) when (e.StatusCode is HttpStatusCode.NotFound)
        {
            CosmologyOutput.Error($"Database with the name {settings.DatabaseName} does not exist");
        }

        return 1;
    }
}