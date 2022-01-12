using System;
using System.Net.Http;
using MassTransit.Testing;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.ChangeFeed.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Functional.Tests.Factory;
using Nomad.Sorter.Infrastructure.Cosmos.Items;
using Nomad.Testing.MassTransit;
using Xunit.Abstractions;

namespace Nomad.Sorter.Functional.Tests.Abstractions;

public abstract class NomadSorterFunctionalTest
{
    protected readonly HttpClient Client;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly MassTransitConsumerInvoker ConsumerInvoker;
    protected readonly IRepository<Parcel> ParcelRepository;
    protected readonly IRepository<ParcelLookupByParcelIdItem> ParcelIdLookupRepository;

    protected NomadSorterFunctionalTest(ITestOutputHelper testOutputHelper)
    {
        var factory = new NomadSorterApplicationFactory(testOutputHelper);
        Client = factory.CreateClient();
        ServiceProvider = factory.Services;
        ConsumerInvoker = ServiceProvider.GetRequiredService<MassTransitConsumerInvoker>();
        ParcelRepository = ServiceProvider.GetRequiredService<IRepository<Parcel>>();
        ParcelIdLookupRepository = ServiceProvider.GetRequiredService<IRepository<ParcelLookupByParcelIdItem>>();
        SetupInMemoryChangeFeeds();
    }
    

    private void SetupInMemoryChangeFeeds() => 
        ServiceProvider.GetRequiredService<InMemoryChangeFeed<Parcel>>().Setup();
    
}