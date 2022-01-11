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
using Nomad.Sorter.Infrastructure.Cosmos;
using Nomad.Sorter.Infrastructure.Cosmos.Items;
using Nomad.Sorter.Infrastructure.Cosmos.Repositories;
using Nomad.Testing;
using Xunit;

namespace Nomad.Sorter.Unit.Tests.Infrastructure.Comsos.Repositories;

public class ParcelRepositoryTests
{
    private readonly AutoMocker _mocker = new();
    private readonly IRepository<Parcel> _parcelRepository;
    private readonly IRepository<ParcelLookupByParcelIdItem> _parcelIdReferenceRepository;
    
    public ParcelRepositoryTests()
    {
        var services = new ServiceCollection()
            .AddInMemoryCosmosRepository()
            .AddSingleton<IParcelRepository, ParcelRepository>();

        var provider = services.BuildServiceProvider();

        _parcelRepository = provider.GetRequiredService<IRepository<Parcel>>();
        _parcelIdReferenceRepository = provider.GetRequiredService<IRepository<ParcelLookupByParcelIdItem>>();

        _mocker.Use(_parcelRepository); 
        _mocker.Use(_parcelIdReferenceRepository);
    }

    private IParcelRepository CreateSut() =>
        _mocker.CreateInstance<ParcelRepository>();

    private static readonly Func<EquivalencyAssertionOptions<Parcel>, EquivalencyAssertionOptions<Parcel>>
        ParcelExclusions = options => options
            .Excluding(x => x.Etag)
            .Excluding(x => x.LastUpdatedTimeRaw)
            .Excluding(x => x.LastUpdatedTimeUtc);

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

    [Fact]
    public async Task GetParcel_ParcelIdForParcelWithIdReference_ReadsParcel()
    {
        //Arrange
        var sut = CreateSut();
        
        var parcel = new Parcel(NomadIdentifiers.ParcelId,
            new DeliveryInformation(Guid.NewGuid().ToString(), "HU117AA"),
            NomadIdentifiers.ClientId);

        await sut.CreateParcel(parcel);
        await _parcelIdReferenceRepository.CreateAsync(parcel.ToParcelLookupByParcelIdItem());
        
        //Act
        var got = await sut.GetParcel(parcel.ParcelId);

        //Assert
        got.Should().BeEquivalentTo(parcel, ParcelExclusions);
    }
    
    [Fact]
    public async Task GetParcel_ParcelIdAndDeliveryRegionIdOfParcelThatExists_ReadsParcel()
    {
        //Arrange
        var sut = CreateSut();
        
        var parcel = new Parcel(NomadIdentifiers.ParcelId,
            new DeliveryInformation(Guid.NewGuid().ToString(), "HU117AA"),
            NomadIdentifiers.ClientId);

        await sut.CreateParcel(parcel);

        //Act
        var got = await sut.GetParcel(parcel.ParcelId, parcel.DeliveryInformation.RegionId);

        //Assert
        got.Should().BeEquivalentTo(parcel, ParcelExclusions);
    }
    
    [Fact]
    public async Task GetParcel_ParcelIdForParcelWithoutIdReference_ThrowsResourceNotFoundException()
    {
        //Arrange
        var sut = CreateSut();
        
        var parcel = new Parcel(NomadIdentifiers.ParcelId,
            new DeliveryInformation(Guid.NewGuid().ToString(), "HU117AA"),
            NomadIdentifiers.ClientId);

        await sut.CreateParcel(parcel);

        //Act
        //Assert
        await Assert.ThrowsAsync<ResourceNotFoundException<Parcel>>(() => sut.GetParcel(parcel.ParcelId).AsTask());
    }
    
    [Fact]
    public async Task GetParcel_ParcelIdForParcelWithLookupButNoParcel_ThrowsResourceNotFoundException()
    {
        //Arrange
        var sut = CreateSut();
        
        var parcel = new Parcel(NomadIdentifiers.ParcelId,
            new DeliveryInformation(Guid.NewGuid().ToString(), "HU117AA"),
            NomadIdentifiers.ClientId);

        await _parcelIdReferenceRepository.CreateAsync(parcel.ToParcelLookupByParcelIdItem());

        //Act
        //Assert
        await Assert.ThrowsAsync<ResourceNotFoundException<Parcel>>(() => sut.GetParcel(parcel.ParcelId).AsTask());
    }
}