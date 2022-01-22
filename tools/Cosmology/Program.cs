
using Cosmology.Commands;
using Cosmology.Commands.Database;
using Cosmology.Settings;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(configurator =>
{
    configurator.AddBranch<DbCommonSettings>("db", dbConfigurator =>
    {
        dbConfigurator.AddCommand<DeleteDatabase>("delete");
        dbConfigurator.AddCommand<ListDatabases>("list");
    });
});

return await app.RunAsync(args);