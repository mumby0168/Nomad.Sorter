namespace Nomad.Sorter.Domain.ValueObjects;

/// <summary>
/// The delivery information for a parcel
/// </summary>
/// <param name="RegionId">The region ID that the postcode falls into</param>
/// <param name="Postcode">The postcode in which the item will be delivered to.</param>
public record DeliveryInformation(
    string RegionId,
    string Postcode
);