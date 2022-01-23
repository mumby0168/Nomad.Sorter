using Convey.CQRS.Events;
using Nomad.Sorter.Application.Events.Inbound;
using Nomad.Sorter.Application.Infrastructure;
using Nomad.Sorter.Domain.Extensions;

namespace Nomad.Sorter.Application.Handlers.Events;

public class ParcelLoadedEventHandler : IEventHandler<ParcelLoadedEvent>
{
    private readonly IBayRepository _bayRepository;
    private readonly IParcelRepository _parcelRepository;

    public ParcelLoadedEventHandler(
        IBayRepository bayRepository,
        IParcelRepository parcelRepository)
    {
        _bayRepository = bayRepository;
        _parcelRepository = parcelRepository;
    }
    
    public async Task HandleAsync(ParcelLoadedEvent parcelLoadedEvent, CancellationToken cancellationToken)
    {
        var parcelId = parcelLoadedEvent.ParcelId.ToParcelId();
        var bayId = parcelLoadedEvent.BayId.ToBayId();
        var bay = await _bayRepository.GetBay(bayId, cancellationToken);
        var deliveryRegionId = bay.GetDeliveryRegionId();

        var parcel = await _parcelRepository.GetParcel(
            parcelId, 
            deliveryRegionId, 
            cancellationToken);

        parcel.Loaded(bayId);

        await _parcelRepository.SaveParcel(parcel, cancellationToken);
    }   
}