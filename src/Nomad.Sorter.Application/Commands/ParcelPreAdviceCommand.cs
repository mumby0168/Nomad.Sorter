using Nomad.Sorter.Application.Abstractions;

namespace Nomad.Sorter.Application.Commands;

public record ParcelPreAdviceCommand(
    string ParcelId,
    string ClientId,
    string DeliveryRegionId
) : ICommand;