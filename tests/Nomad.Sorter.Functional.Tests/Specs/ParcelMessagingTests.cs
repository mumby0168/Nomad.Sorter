using System;
using System.Threading.Tasks;
using FluentAssertions;
using Nomad.Sorter.Application.Commands;
using Nomad.Sorter.Application.Events.Inbound;
using Nomad.Sorter.Functional.Tests.Abstractions;
using Nomad.Sorter.Infrastructure.Messaging.Consumers;
using Nomad.Testing.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Nomad.Sorter.Functional.Tests.Specs;

public class ParcelPreAdviceSpecTests : NomadSorterFunctionalTest
{
    public ParcelPreAdviceSpecTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }
    
    [Fact]
    public async Task Pre_Advice_For_Parcel_Consumed_Correctly()
    {
        //Arrange
        var command = new ParcelPreAdviceCommand(
            Guid.NewGuid().ToString("N"),
            "CLIENT123",
            Guid.NewGuid().ToString(),
            "HU115TG"
        );

        //Act
        await ConsumerInvoker.Invoke<ParcelPreAdviceCommand, ParcelPreAdviceCommandConsumer>(command);

        //Assert
        var parcel = await ParcelRepository.GetAsync(command.ParcelId, command.DeliveryRegionId);
        parcel.Should().BeValidParcelFor(command);

        var parcelLookupByParcelIdItem = await ParcelIdLookupRepository.GetAsync(parcel.Id);
        parcelLookupByParcelIdItem.DeliveryRegionId.Should().Be(command.DeliveryRegionId);
    }

    [Fact]
    public async Task Parcel_With_Pre_Advice_When_Inducted_Message_Consumed_Parcel_Is_Inducted_Correctly()
    {
        //Arrange
        var command = new ParcelPreAdviceCommand(
            Guid.NewGuid().ToString("N"),
            "CLIENT123",
            Guid.NewGuid().ToString(),
            "HU115TG"
        );

        await ConsumerInvoker.Invoke<ParcelPreAdviceCommand, ParcelPreAdviceCommandConsumer>(command);
        
        var parcelInductedEvent = new ParcelInductedEvent(
            command.ParcelId
        );

        //Act
        await ConsumerInvoker.Invoke<ParcelInductedEvent, ParcelInductedEventConsumer>(parcelInductedEvent);

        //Assert
        var parcel = await ParcelRepository.GetAsync(command.ParcelId, command.DeliveryRegionId);
        parcel.Should().BeValidParcelFor(parcelInductedEvent);
    }
}