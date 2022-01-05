using System;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.Enums;
using Nomad.Sorter.Domain.Identitifiers;
using Nomad.Sorter.Domain.ValueObjects;
using Xunit;

namespace Nomad.Sorter.Unit.Tests.Domain.Entities;

public class BayTests
{
    [Fact]
    public void JsonConstructor_JsonData_DeserializesCorrectly()
    {
        //Arrange   
        var json = JObject.FromObject(new
        {
            _etag = "123r454",
            createdTimeUtc = DateTime.UtcNow.ToString("O"),
            _ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            id = "YRK001",
            status = "Servicing",
            type = nameof(Bay),
            partitionKey = "Bay",
            dockingInformation = new DockingInformation(
                "YX21RFD",
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(30),
                50,
                Guid.NewGuid().ToString()
            )
        }).ToString();

        //Act
        var bay = JsonConvert.DeserializeObject<Bay>(json);

        //Assert
        bay.Id.Should().Be("YRK001");
        bay.BayId.Value.Should().Be("YRK001");
        bay.Status.Should().Be(BayStatus.Servicing);
        bay.Type.Should().Be(nameof(Bay));
        bay.PartitionKey.Should().Be(nameof(Bay));
        bay.Etag.Should().Be("123r454");
        bay.DockingInformation.Should().NotBeNull();
        bay.DockingInformation!.ParcelCapacity.Should().Be(50);
        bay.DockingInformation.DepartingUtc.Should()
            .BeCloseTo(DateTime.UtcNow.AddMinutes(30), TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Constructor_BayId_CreatesBay()
    {
        //Arrange
        var bayId = new BayId("YRK001");
        
        //Act
        var bay = new Bay(bayId);
        
        //Assert
        bay.Id.Should().Be("YRK001");
        bay.BayId.Should().Be(bayId);
        bay.Status.Should().Be(BayStatus.Empty);
        bay.PartitionKey.Should().Be(nameof(Bay));
        bay.DockingInformation.Should().BeNull();
    }
}