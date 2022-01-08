using System;
using System.Net.Http;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Extensions.DependencyInjection;
using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Functional.Tests.Factory;
using Nomad.Sorter.Infrastructure.Cosmos.Items;
using Nomad.Sorter.Infrastructure.Cosmos.Repositories;
using Nomad.Testing.ChangeFeed;
using Nomad.Testing.MassTransit;

namespace Nomad.Sorter.Functional.Tests.Abstractions;

public abstract class NomadSorterFunctionalTest
{
    protected readonly HttpClient Client;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly MassTransitConsumerInvoker ConsumerInvoker;
    protected readonly IRepository<Parcel> ParcelRepository;
    protected readonly ChangeFeedProcessorInvoker<Parcel> ParcelChangeFeedInvoker;
    protected readonly IRepository<ParcelLookupByParcelIdItem> ParcelIdLookupRepository;

    protected NomadSorterFunctionalTest()
    {
        var factory = new NomadSorterApplicationFactory();
        Client = factory.CreateClient();
        ServiceProvider = factory.Services;
        ConsumerInvoker = ServiceProvider.GetRequiredService<MassTransitConsumerInvoker>();
        ParcelRepository = ServiceProvider.GetRequiredService<IRepository<Parcel>>();
        ParcelChangeFeedInvoker = ServiceProvider.GetRequiredService<ChangeFeedProcessorInvoker<Parcel>>();
        ParcelIdLookupRepository = ServiceProvider.GetRequiredService<IRepository<ParcelLookupByParcelIdItem>>();
    }
}