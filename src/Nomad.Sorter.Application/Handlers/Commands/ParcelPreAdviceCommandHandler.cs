using Nomad.Abstractions.Cqrs;
using Nomad.Sorter.Application.Commands;
using Nomad.Sorter.Application.Infrastructure;
using Nomad.Sorter.Domain.Factories;

namespace Nomad.Sorter.Application.Handlers.Commands;

public class ParcelPreAdviceCommandHandler : ICommandHandler<ParcelPreAdviceCommand>
{
    private readonly IParcelFactory _parcelFactory;
    private readonly IParcelRepository _parcelRepository;

    public ParcelPreAdviceCommandHandler(IParcelFactory parcelFactory, IParcelRepository parcelRepository)
    {
        _parcelFactory = parcelFactory;
        _parcelRepository = parcelRepository;
    }
    
    public async Task Handle(ParcelPreAdviceCommand command, CancellationToken cancellationToken)
    {
        var (parcelId, clientId, deliveryRegionId, deliveryPostCode) = command;

        var parcel = _parcelFactory.Create(parcelId, clientId, deliveryRegionId, deliveryPostCode);

        await _parcelRepository.CreateParcel(parcel, cancellationToken);
    }
}