using System;
using System.Threading.Tasks;
using CleanArchitecture.Exceptions;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Extensions.DependencyInjection;
using Moq.AutoMock;
using Nomad.Sorter.Application.Infrastructure;
using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.ValueObjects;
using Nomad.Sorter.Infrastructure.Cosmos.Repositories;
using Nomad.Testing;
using Xunit;

namespace Nomad.Sorter.Unit.Tests.Infrastructure.Comsos.Repositories;

public class ParcelRepositoryTests
{
    private readonly AutoMocker _mocker = new();
    private readonly IRepository<Parcel> _parcelRepository;

    private IParcelRepository CreateSut() => _mocker.CreateInstance<ParcelRepository>();

    private static readonly Func<EquivalencyAssertionOptions<Parcel>, EquivalencyAssertionOptions<Parcel>> ParcelExclusions = options => options
        .Excluding(x => x.Etag)
        .Excluding(x => x.LastUpdatedTimeRaw)
        .Excluding(x => x.LastUpdatedTimeUtc);

    public ParcelRepositoryTests()
    {
        var services = new ServiceCollection()
            .AddInMemoryCosmosRepository()
            .AddSingleton<IParcelRepository, ParcelRepository>();

        var provider = services.BuildServiceProvider();

        _parcelRepository = provider.GetRequiredService<IRepository<Parcel>>();

        _mocker.Use(_parcelRepository);
    }

    [Fact]
    public async Task CreateParcel_ValidParcel_ParcelIsCreated()
    {
        //Arrange
        var sut = CreateSut();

        var parcel = new Parcel(NomadIdentifiers.ParcelId,
            new DeliveryInformation(Guid.NewGuid().ToString(), "HU117AA"),
            NomadIdentifiers.ClientId);

        //Act
        await sut.CreateParcel(parcel);

        //Assert
        var createdParcel = await _parcelRepository.GetAsync(parcel.Id, parcel.DeliveryInformation.RegionId);
        createdParcel.Should().BeEquivalentTo(parcel, ParcelExclusions);
    }


    [Fact]
    public async Task CreateParcel_ParcelThatAlreadyExists_ThrowsResourceAlreadyExistsException()
    {
        //Arrange
        var sut = CreateSut();

        var parcel = new Parcel(NomadIdentifiers.ParcelId,
            new DeliveryInformation(Guid.NewGuid().ToString(), "HU117AA"),
            NomadIdentifiers.ClientId);

        await _parcelRepository.CreateAsync(parcel);

        //Act
        //Assert
        await Assert.ThrowsAsync<ResourceExistsException<Parcel>>(() => sut.CreateParcel(parcel).AsTask());
    }
}