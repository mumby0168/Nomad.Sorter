using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Domain.Extensions;
using Nomad.Sorter.Domain.Identitifiers;
using Nomad.Sorter.Domain.ValueObjects;

namespace Nomad.Sorter.Domain.Factories;

public class ParcelFactory : IParcelFactory
{
    public IParcel Create(string parcelId, string clientId, string deliveryRegionId, string deliveryPostcode) =>
        new Parcel(parcelId.ToParcelId(), new DeliveryInformation(deliveryRegionId, deliveryPostcode), clientId.ToClientId());
}