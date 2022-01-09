using System;
using CleanArchitecture.Exceptions;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.Enums;
using Nomad.Sorter.Domain.Identitifiers;
using Nomad.Sorter.Domain.ValueObjects;
using Xunit;

namespace Nomad.Sorter.Unit.Tests.Domain.Entities;


public class ParcelTests
{
    [Fact]
    public void JsonConstructor_JsonData_DeserializesCorrectly()
    {
        //Arrange
        var regionId = Guid.NewGuid().ToString();
        var parcelId = Guid.NewGuid().ToString("N");
        
        var json = JObject.FromObject(new
        {
            _etag = "123r454",
            createdTimeUtc = DateTime.UtcNow.ToString("O"),
            _ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            id = parcelId,
            status = "InTransit",
            type = nameof(Parcel),
            partitionKey = regionId,
            deliveryInformation = new DeliveryInformation(regionId, "HU56TGH")
        }).ToString();

        //Act
        var parcel = JsonConvert.DeserializeObject<Parcel>(json);

        //Assert
        parcel.Id.Should().Be(parcelId);
        parcel.Status.Should().Be(ParcelStatus.InTransit);
        parcel.Type.Should().Be(nameof(Parcel));
        parcel.PartitionKey.Should().Be(regionId);
        parcel.Etag.Should().Be("123r454");
        parcel.ParcelId.Value.Should().Be(parcelId);
        parcel.DeliveryInformation.RegionId.Should().Be(regionId);
        parcel.DeliveryInformation.Postcode.Should().Be("HU56TGH");
    }

    [Fact]
    public void Constructor_Data_CreatesParcel()
    {
        //Arrange
        var deliveryInformation = new DeliveryInformation(Guid.NewGuid().ToString(), "HU12F43");
        var parcelId = new ParcelId(Guid.NewGuid().ToString("N"));
        var clientId = new ClientId("CAMP123");

        //Act
        var parcel = new Parcel(parcelId, deliveryInformation, clientId);
        
        //Assert
        parcel.Id.Should().Be(parcelId.ToString());
        parcel.PartitionKey.Should().Be(deliveryInformation.RegionId);
        parcel.Status.Should().Be(ParcelStatus.PreAdvice);
        parcel.DeliveryInformation.Should().Be(deliveryInformation);
        parcel.ClientId.Should().Be(clientId);
    }

    [Fact]
    public void Inducted_ParcelNotAlreadyInducted_SetsParcelInductProperties()
    {
        //Arrange
        var deliveryInformation = new DeliveryInformation(Guid.NewGuid().ToString(), "HU12F43");
        var parcelId = new ParcelId(Guid.NewGuid().ToString("N"));
        var clientId = new ClientId("CAMP123");
        var parcel = new Parcel(parcelId, deliveryInformation, clientId);

        //Act
        parcel.Inducted();

        parcel.Status.Should().Be(ParcelStatus.Inducted);
        parcel.InductedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
    
    [Fact]
    public void Inducted_ParcelAlreadyInducted_ThrowsDomainException()
    {
        //Arrange
        var deliveryInformation = new DeliveryInformation(Guid.NewGuid().ToString(), "HU12F43");
        var parcelId = new ParcelId(Guid.NewGuid().ToString("N"));
        var clientId = new ClientId("CAMP123");
        var parcel = new Parcel(parcelId, deliveryInformation, clientId);
        parcel.Inducted();

        //Act
        Assert.Throws<DomainException<Parcel>>(() => parcel.Inducted());
    }
}