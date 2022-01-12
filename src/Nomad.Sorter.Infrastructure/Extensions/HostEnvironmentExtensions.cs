using Microsoft.Extensions.Hosting;

namespace Nomad.Sorter.Infrastructure.Extensions;

public static class HostEnvironmentExtensions
{
    public static bool IsNotFunctionalTests(this IHostEnvironment hostEnvironment) =>
        hostEnvironment.EnvironmentName is not "FunctionalTests";
}