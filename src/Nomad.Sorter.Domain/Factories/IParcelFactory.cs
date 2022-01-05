using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Domain.Identitifiers;

namespace Nomad.Sorter.Domain.Factories;

public interface IParcelFactory
{
    IParcel Create(string parcelId, string clientId, string deliveryRegionId, string deliveryPostcode);
}