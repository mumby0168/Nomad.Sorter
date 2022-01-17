using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Domain.Identitifiers;

namespace Nomad.Sorter.Application.Infrastructure;

public interface IParcelRepository
{
    ValueTask CreateParcel(
        IParcel parcel, 
        CancellationToken cancellationToken = default);
    
    ValueTask<IParcel> GetParcel(
        ParcelId parcelId, 
        CancellationToken cancellationToken = default);

    ValueTask<IParcel> GetParcel(
        ParcelId parcelId, 
        string deliveryRegionId,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<IParcel> GetParcelsWithDeliveryRegionId(
        string deliveryRegionId, 
        int max, 
        CancellationToken cancellationToken = default);

    ValueTask SaveParcel(
        IParcel parcel, 
        CancellationToken cancellationToken = default);
}