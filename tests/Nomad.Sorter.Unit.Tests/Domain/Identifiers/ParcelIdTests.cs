using System;
using System.Collections;
using System.Collections.Generic;
using CleanArchitecture.Exceptions;
using Nomad.Sorter.Domain.Identitifiers;
using Xunit;

namespace Nomad.Sorter.Unit.Tests.Domain.Identifiers;

public class ParcelIdTests
{
    [Theory]
    [ClassData(typeof(ParcelIdTestData))]
    public void Ctor_InvalidValue_ThrowsDomainException(bool shouldThrow, string bayId)
    {
        //Arrange
        //Act
        //Assert
        if (shouldThrow)
        {
            Assert.Throws<DomainException<ParcelId>>(() => new ParcelId(bayId));
        }
        else
        {
            var _ = new ParcelId(bayId);
        }   
    }
}

public class ParcelIdTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] {false, Guid.NewGuid().ToString("N")};
        yield return new object[] {true, Guid.NewGuid().ToString()};
        yield return new object[] {true, "YRK001"};
        yield return new object[] {true, $"{Guid.NewGuid().ToString()}-{Guid.NewGuid().ToString()}"};
        yield return new object[] {true, ""};
        yield return new object[] {true, null!};
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}