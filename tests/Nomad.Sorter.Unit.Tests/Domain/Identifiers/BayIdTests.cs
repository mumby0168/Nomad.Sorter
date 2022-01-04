using System.Collections;
using System.Collections.Generic;
using CleanArchitecture.Exceptions;
using Nomad.Sorter.Domain.Identitifiers;
using Xunit;

namespace Nomad.Sorter.Unit.Tests.Domain.Identifiers;

public class BayIdTests
{
    [Theory]
    [ClassData(typeof(BayIdTestData))]
    public void Ctor_InvalidValue_ThrowsDomainException(bool shouldThrow, string bayId)
    {
        //Arrange
        //Act
        //Assert
        if (shouldThrow)
        {
            Assert.Throws<DomainException<BayId>>(() => new BayId(bayId));
        }
        else
        {
            var _ = new BayId(bayId);
        }   
    }
}

public class BayIdTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] {true, "123"};
        yield return new object[] {true, "AB1234"};
        yield return new object[] {false, "YRK001"};
        yield return new object[] {true, "YRK0011"};
        yield return new object[] {true, ""};
        yield return new object[] {true, null!};
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}