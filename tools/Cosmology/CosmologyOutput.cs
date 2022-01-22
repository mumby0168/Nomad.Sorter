using Spectre.Console;

namespace Cosmology;

public static class CosmologyOutput
{
    public static void Error(string message) =>
        AnsiConsole.MarkupLine($"[red]{message}[/]");
    
    public static void Success(string message) =>
        AnsiConsole.MarkupLine($"[green]{message}[/]");
}