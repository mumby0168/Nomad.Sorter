using System;
using System.Threading.Tasks;
using Microsoft.Azure.CosmosRepository;
using Moq;
using Moq.AutoMock;
using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.ValueObjects;
using Nomad.Sorter.Infrastructure.Cosmos.ChangeFeed;
using Nomad.Sorter.Infrastructure.Cosmos.ChangeFeed.Processors;
using Nomad.Sorter.Infrastructure.Cosmos.Items;
using Nomad.Testing;
using Xunit;

namespace Nomad.Sorter.Unit.Tests.Infrastructure.Comsos.ChangeFeed.Processors;

public class ParcelChangeFeedProcessorTests
{
    private readonly AutoMocker _mocker = new();
    private readonly Mock<IRepository<ParcelIdLookup>> _lookupRepository;

    private IChangeFeedItemProcessor<Parcel> CreateSut() => _mocker.CreateInstance<ParcelChangeFeedProcessor>();

    public ParcelChangeFeedProcessorTests() =>
        _lookupRepository = _mocker.GetMock<IRepository<ParcelIdLookup>>();

    [Fact]
    public async Task HandleAsync_ParcelAtStatusPreAdvice_InsertLookupRecord()
    {
        //Arrange
        var sut = CreateSut();

        var parcel = new Parcel(NomadIdentifiers.ParcelId,
            new DeliveryInformation(Guid.NewGuid().ToString(), "HU117AA"),
            NomadIdentifiers.ClientId);

        //Act
        await sut.HandleAsync(parcel, default);

        //Assert
        _lookupRepository.Verify(o =>
            o.UpdateAsync(
                It.Is<ParcelIdLookup>(x =>
                    x.Id == parcel.Id && x.PartitionKey == parcel.Id &&
                    x.DeliveryRegionId == parcel.DeliveryInformation.RegionId), default, false));
    }
}