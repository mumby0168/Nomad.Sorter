using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using Nomad.Sorter.Application.Events.Inbound;
using Nomad.Sorter.Application.Events.Outbound;
using Nomad.Sorter.Application.Infrastructure;
using Nomad.Sorter.Application.Logging;
using Nomad.Sorter.Domain.Extensions;

namespace Nomad.Sorter.Application.Handlers.Events;

public class VehicleDockedEventHandler : IEventHandler<VehicleDockedEvent>
{
    private readonly ILogger<VehicleDockedEventHandler> _logger;
    private readonly IBayRepository _bayRepository;
    private readonly IParcelRepository _parcelRepository;
    private readonly IDispatcher _dispatcher;

    public VehicleDockedEventHandler(
        ILogger<VehicleDockedEventHandler> logger,
        IBayRepository bayRepository,
        IParcelRepository parcelRepository,
        IDispatcher dispatcher)
    {
        _logger = logger;
        _bayRepository = bayRepository;
        _parcelRepository = parcelRepository;
        _dispatcher = dispatcher;
    }

    public async Task HandleAsync(VehicleDockedEvent vehicleDockedEvent, CancellationToken cancellationToken)
    {
        var (vehicleRegistration,
            deliveryRegionId,
            bayId,
            parcelCapacity,
            departingAtUtc,
            dockedAtUtc) = vehicleDockedEvent;

        var bay = await _bayRepository.GetBay(bayId.ToBayId(), cancellationToken);

        bay.DockVehicle(
            vehicleRegistration,
            dockedAtUtc,
            departingAtUtc,
            parcelCapacity,
            deliveryRegionId);

        await _bayRepository.SaveBay(bay, cancellationToken);
        _logger.LogVehicleDocked(vehicleRegistration, bayId);

        var parcelAssociatedEvents = new List<ParcelAssociatedEvent>();

        await foreach (var parcel in _parcelRepository.GetParcelsWithDeliveryRegionId(deliveryRegionId, parcelCapacity,
                           cancellationToken: cancellationToken))
        {
            parcel.Associate(bay);
            parcelAssociatedEvents.Add(new(parcel.ParcelId, vehicleRegistration, bayId));

            await _parcelRepository.SaveParcel(parcel, cancellationToken);
            _logger.LogParcelAssociated(parcel.ParcelId, bayId);
        }

        await _dispatcher.DispatchParcelAssociatedEvents(parcelAssociatedEvents);
    }
}