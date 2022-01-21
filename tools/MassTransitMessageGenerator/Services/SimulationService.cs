using System.Collections.Concurrent;
using MassTransitMessageGenerator.Models;
using Microsoft.AspNetCore.Identity;
using Nomad.Sorter.Application.Commands;
using Nomad.Sorter.Application.Events.Inbound;

namespace MassTransitMessageGenerator.Services;

public class SimulationService : ISimulationService
{
    private readonly IMessageQueuingService _messageQueuingService;
    private readonly ILogger<SimulationService> _logger;
    private Guid _deliveryRegionId = Guid.NewGuid();

    public SimulationService(
        IMessageQueuingService messageQueuingService,
        ILogger<SimulationService> logger)
    {
        _messageQueuingService = messageQueuingService;
        _logger = logger;
    }

    public bool IsSimulationInProgress { get; private set; } = false;

    public Guid SimulationId { get; private set; } = Guid.NewGuid();

    public bool TryStart(StartSimulationRequest startSimulationRequest)
    {
        if (IsSimulationInProgress)
        {
            return false;
        }

        _deliveryRegionId = Guid.NewGuid();
        SimulationId = Guid.NewGuid();
        IsSimulationInProgress = true;
        Parcels = new ConcurrentBag<SimulatedParcel>();

        _logger.LogInformation("Starting new simulation {SimulationId} working with delivery region {DeliveryRegionId}",
            SimulationId, _deliveryRegionId);

        for (int i = 0; i < startSimulationRequest.ParcelsSimulation; i++)
        {
            var simulationParcel = new SimulatedParcel
            {
                Id = Guid.NewGuid().ToString("N"),
                DeliveryRegionId = _deliveryRegionId.ToString(),
            };
            Parcels.Add(simulationParcel);
            _messageQueuingService.Operations.Enqueue(new ParcelPreAdviceCommand(simulationParcel.Id,
                "CLIENT123", simulationParcel.DeliveryRegionId, "SOME_POST_CODE"));
        }


        Task.Run(() =>
        {
            Task.Delay(3000);
            foreach (var simulatedParcel in Parcels)
            {
                _messageQueuingService.Operations.Enqueue(new ParcelInductedEvent(simulatedParcel.Id));
            }
        });

        return true;
    }

    public void Cancel()
    {
        throw new NotImplementedException();
    }

    public ConcurrentBag<SimulatedParcel> Parcels { get; set; }
    public string DeliveryRegionId { get; private set; }
}