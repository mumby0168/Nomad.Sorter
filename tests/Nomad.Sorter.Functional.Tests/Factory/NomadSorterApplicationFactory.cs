using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Nomad.Testing.MassTransit;
using Xunit.Abstractions;

namespace Nomad.Sorter.Functional.Tests.Factory;

public class NomadSorterApplicationFactory : WebApplicationFactory<Program>
{
    private readonly ITestOutputHelper _testOutputHelper;

    public NomadSorterApplicationFactory(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }   


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("FunctionalTests");
        builder.ConfigureServices(services =>
        {
            services.AddInMemoryCosmosRepository();
            services.AddMassTransitTestServices();
            services.AddSingleton(_testOutputHelper);
            services.AddLogging(loggingBuilder => loggingBuilder.AddXUnit(_testOutputHelper));
        });
    }
}