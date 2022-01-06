using System;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Nomad.Sorter.Application.Commands;
using Nomad.Sorter.Application.Handlers.Commands;
using Nomad.Sorter.Application.Infrastructure;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Domain.Factories;
using Nomad.Testing;
using Xunit;

namespace Nomad.Sorter.Unit.Tests.Application.Handlers.Commands;

public class ParcelPreAdviceCommandHandlerTests
{
    private readonly AutoMocker _mocker = new();
    private readonly Mock<IParcelFactory> _parcelFactory;
    private readonly Mock<IParcelRepository> _parcelRepository;

    private ParcelPreAdviceCommandHandler CreateSut() => _mocker.CreateInstance<ParcelPreAdviceCommandHandler>();

    public ParcelPreAdviceCommandHandlerTests()
    {
        _parcelFactory = _mocker.GetMock<IParcelFactory>();
        _parcelRepository = _mocker.GetMock<IParcelRepository>();
    }

    [Fact]
    public async Task Handle_ParcelPreAdviceCommand_CreatesParcel()
    {
        //Arrange
        var sut = CreateSut();

        var parcelId = NomadIdentifiers.ParcelId;
        var clientId = NomadIdentifiers.ClientId;
        var deliveryRegionId = Guid.NewGuid().ToString();
        var deliveryPostCode = "HU10 7LL";

        var parcel = new Mock<IParcel>();

        _parcelFactory
            .Setup(o => o.Create(parcelId, clientId, deliveryRegionId, deliveryPostCode))
            .Returns(parcel.Object);
        
        var command = new ParcelPreAdviceCommand(parcelId, clientId, deliveryRegionId, deliveryPostCode);
        

        //Act
        await sut.Handle(command, default);

        //Assert
        _parcelRepository.Verify(o => o.CreateParcel(parcel.Object, default), Times.Once);
    }
}