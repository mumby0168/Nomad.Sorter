using Spectre.Console.Cli;

namespace Cosmology.Settings;

public class DbCommonSettings : CommandSettings
{
    [CommandOption("-d|--database-name <databaseName>")]
    public string DatabaseName { get; set; } = default!;
}