using Microsoft.Extensions.DependencyInjection;

namespace Nomad.Testing.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RemoveNamespaceServices(this IServiceCollection services, string namespacePrefix)
    {
        var descriptors =
            services.Where(d =>
                d.ServiceType.Namespace is not null &&
                d.ServiceType.Namespace.StartsWith(namespacePrefix, StringComparison.OrdinalIgnoreCase)).ToList();

        foreach (var d in descriptors)
        {
            services.Remove(d);
        }

        return services;
    }
}