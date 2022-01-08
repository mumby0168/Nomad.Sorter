using System;
using System.Threading.Tasks;
using FluentAssertions;
using Nomad.Sorter.Application.Commands;
using Nomad.Sorter.Functional.Tests.Abstractions;
using Nomad.Sorter.Infrastructure.Messaging.Consumers;
using Nomad.Testing.Extensions;
using Xunit;

namespace Nomad.Sorter.Functional.Tests.Specs;

public class ParcelPreAdviceSpecTests : NomadSorterFunctionalTest
{
    [Fact]
    public async Task Pre_Advice_Received_Is_Consumed_Correctly()
    {
        //Arrange
        var command = new ParcelPreAdviceCommand(Guid.NewGuid().ToString("N"),
            "CLIENT123", Guid.NewGuid().ToString(), "HU115TG");
        
        //Act
        await ConsumerInvoker.Invoke<ParcelPreAdviceCommand, ParcelPreAdviceCommandConsumer>(command);
        await ParcelChangeFeedInvoker.InvokeFor(command.ParcelId, command.DeliveryRegionId);

        //Assert
        var parcel = await ParcelRepository.GetAsync(command.ParcelId, command.DeliveryRegionId);
        parcel.Should().BeValidParcelFor(command);

        var parcelLookupByParcelIdItem = await ParcelIdLookupRepository.GetAsync(parcel.Id);
        parcelLookupByParcelIdItem.DeliveryRegionId.Should().Be(command.DeliveryRegionId);
    }
}