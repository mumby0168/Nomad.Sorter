using System;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Nomad.Sorter.Application.Events.Inbound;
using Nomad.Sorter.Application.Handlers.Events;
using Nomad.Sorter.Application.Infrastructure;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Testing;
using Xunit;

namespace Nomad.Sorter.Unit.Tests.Application.Handlers.Events;

public class ParcelInductedEventHandlerTests
{
    private readonly AutoMocker _mocker = new();
    private readonly Mock<IParcelRepository> _parcelRepository;

    private ParcelInductedEventHandler CreateSut() => _mocker.CreateInstance<ParcelInductedEventHandler>();

    public ParcelInductedEventHandlerTests() =>
        _parcelRepository = _mocker.GetMock<IParcelRepository>();

    [Fact]
    public async Task Handle_ParcelInductedEvent_SuccessfullyInductsParcel()
    {
        //Arrange
        var sut = CreateSut();

        var parcelId = NomadIdentifiers.ParcelId;

        var inductedEvent = new ParcelInductedEvent(parcelId.ToString(), 
            NomadIdentifiers.ClientId.ToString());

        var parcel = new Mock<IParcel>();

        _parcelRepository
            .Setup(o => o.GetParcel(parcelId, default))
            .ReturnsAsync(parcel.Object);

        //Act
        await sut.Handle(inductedEvent, default);

        //Assert
        _parcelRepository.Verify(o => o.SaveParcel(parcel.Object, default));
        parcel.Verify(o => o.Inducted());
    }
}