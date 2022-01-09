using FluentAssertions;
using FluentAssertions.Primitives;
using Nomad.Sorter.Application.Commands;
using Nomad.Sorter.Application.Events.Inbound;
using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Domain.Enums;
using Nomad.Sorter.Domain.Extensions;
using Nomad.Sorter.Domain.ValueObjects;

namespace Nomad.Testing.Extensions;

public static class ParcelExtensions 
{
    public static ParcelAssertions Should(this IParcel instance) => new(instance);
    public static ParcelAssertions Should(this Parcel instance) => new(instance);
}

public class ParcelAssertions : 
    ReferenceTypeAssertions<IParcel, ParcelAssertions>
{
    public ParcelAssertions(IParcel instance)
        : base(instance)
    {
    }

    protected override string Identifier => "parcel";

    public AndConstraint<ParcelAssertions> BeValidParcelFor(ParcelPreAdviceCommand command)
    {
        var parcel = Subject as Parcel;
        
        parcel!.Should().NotBeNull();
        parcel!.Id.Should().Be(parcel.ParcelId);
        parcel.PartitionKey.Should().Be(command.DeliveryRegionId);
        parcel.Status.Should().Be(ParcelStatus.PreAdvice);
        parcel.ClientId.Should().Be(command.ClientId.ToClientId());
        parcel.DeliveryInformation.Postcode.Should().Be(command.DeliveryPostCode);
        parcel.DeliveryInformation.RegionId.Should().Be(command.DeliveryRegionId);

        return new AndConstraint<ParcelAssertions>(this);
    }
    
    public AndConstraint<ParcelAssertions> BeValidParcelFor(ParcelInductedEvent parcelInductedEvent)
    {
        var parcel = Subject as Parcel;
        
        parcel!.Should().NotBeNull();
        parcel!.Id.Should().Be(parcel.ParcelId);
        parcel.Status.Should().Be(ParcelStatus.Inducted);
        parcel.InductedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

        return new AndConstraint<ParcelAssertions>(this);
    }
}