using Nomad.Abstractions.Cqrs;

namespace Nomad.Sorter.Application.Commands;

public record ParcelPreAdviceCommand(
    string ParcelId,
    string ClientId,
    string DeliveryRegionId,
    string DeliveryPostCode
) : ICommand;