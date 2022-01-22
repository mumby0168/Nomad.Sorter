using Cosmology.Settings;
using Microsoft.Azure.Cosmos;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Cosmology.Commands.Database;

public class ListDatabases : AsyncCommand<DbCommonSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, DbCommonSettings settings)
    {
        var client = CosmologyClient.CosmosDbClient;

        var iterator = client.GetDatabaseQueryIterator<DatabaseProperties>("SELECT * FROM c");

        var table = new Table()
            .LeftAligned()
            .Title("Databases");

        table.AddColumn("Name");
        table.AddColumn("Last Modified");

        await AnsiConsole.Live(table).StartAsync(async (ctx) =>
        {
            while (iterator.HasMoreResults)
            {
                var results = await iterator.ReadNextAsync();
                foreach (var database in results)
                {
                    table.AddRow(database.Id, database.LastModified?.ToString() ?? "n/a");
                    ctx.Refresh();
                }
            }
        });

        return 1;
    }
}