using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nomad.Testing.ChangeFeed;
using Nomad.Testing.MassTransit;

namespace Nomad.Sorter.Functional.Tests.Factory;

public class NomadSorterApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("FunctionalTests");
        builder.ConfigureServices(services =>
        {
            services.AddInMemoryCosmosRepository();
            services.AddChangeFeedProcessorTestInvokers();
            services.AddMassTransitTestServices();
        });
    }
}