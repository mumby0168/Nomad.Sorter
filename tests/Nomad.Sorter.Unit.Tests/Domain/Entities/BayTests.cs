using System;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.Enums;
using Nomad.Sorter.Domain.Identitifiers;
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
            partitionKey = "Bay"
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

    }
}