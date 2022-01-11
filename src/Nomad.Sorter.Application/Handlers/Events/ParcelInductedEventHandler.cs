using Nomad.Abstractions.Cqrs;
using Nomad.Sorter.Application.Events.Inbound;
using Nomad.Sorter.Application.Infrastructure;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Domain.Extensions;

namespace Nomad.Sorter.Application.Handlers.Events;

public class ParcelInductedEventHandler : IEventHandler<ParcelInductedEvent>
{
    private readonly IParcelRepository _parcelRepository;
    
    public ParcelInductedEventHandler(IParcelRepository parcelRepository) => 
        _parcelRepository = parcelRepository;

    public async Task Handle(ParcelInductedEvent parcelInductedEvent, CancellationToken cancellationToken = default)
    {
        var parcel = await _parcelRepository.GetParcel(parcelInductedEvent.ParcelId.ToParcelId(), cancellationToken);

        parcel.Inducted();
        
        //TODO: See if there any bays waiting to be filled for the given parcel.DeliveryInformation.RegionId, if so associate the parcel with the given bay.

        await _parcelRepository.SaveParcel(parcel, cancellationToken);
    }
}