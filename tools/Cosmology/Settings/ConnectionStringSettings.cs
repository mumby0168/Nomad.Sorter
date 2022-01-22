using Spectre.Console.Cli;

namespace Cosmology.Settings;

public class ConnectionStringSettings : CommandSettings
{
    [CommandArgument(0, "<connectionString>")]
    public string ConnectionString { get; set; } = default!;
}