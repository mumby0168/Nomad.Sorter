using System.Collections.Concurrent;
using Nomad.Simulator.Models;
using Nomad.Sorter.Application.Commands;
using Nomad.Sorter.Application.Events.Inbound;

namespace Nomad.Simulator.Services;

public class SimulationService : ISimulationService
{
    private readonly Queue<object> _queue;
    private readonly ILogger<SimulationService> _logger;

    public SimulationService(
        Queue<object> queue,
        ILogger<SimulationService> logger)
    {
        _queue = queue;
        _logger = logger;
        DeliveryRegionId = Guid.NewGuid().ToString();
        Parcels = new ConcurrentBag<SimulatedDto>();
    }

    public bool IsSimulationInProgress { get; private set; }

    public Guid SimulationId { get; private set; } = Guid.NewGuid();

    public bool TryStart(StartSimulationCommand startSimulationCommand)
    {
        if (IsSimulationInProgress)
        {
            return false;
        }

        DeliveryRegionId = Guid.NewGuid().ToString();
        SimulationId = Guid.NewGuid();
        IsSimulationInProgress = true;
        Parcels.Clear();

        _logger.LogInformation("Starting new simulation {SimulationId} working with delivery region {DeliveryRegionId}",
            SimulationId, DeliveryRegionId);

        for (int i = 0; i < startSimulationCommand.ParcelsInSimulation; i++)
        {
            var simulationParcel = new SimulatedDto
            {
                Id = Guid.NewGuid().ToString("N"),
                DeliveryRegionId = DeliveryRegionId,
            };
            Parcels.Add(simulationParcel);
            _queue.Enqueue(new ParcelPreAdviceCommand(simulationParcel.Id,
                "CLIENT123", simulationParcel.DeliveryRegionId, "SOME_POST_CODE"));
        }


        Task.Run(async () =>
        {
            await Task.Delay(startSimulationCommand.InductDelayInSeconds * 1000);
            
            _logger.LogInformation("Starting parcel's induction");
            
            foreach (var simulatedParcel in Parcels)
            {
                _queue.Enqueue(new ParcelInductedEvent(simulatedParcel.Id));
            }
        });

        return true;
    }

    public bool DockVehicle(SimulateVehicleDockingCommand dockingCommand)
    {
        if (!IsSimulationInProgress)
        {
            return false;
        }

        var dockedEvent = new VehicleDockedEvent(
            dockingCommand.Registration,
            dockingCommand.DeliveryRegionId,
            dockingCommand.BayId,
            dockingCommand.Capacity,
            DateTime.UtcNow.AddMinutes(10),
            DateTime.UtcNow);

        _queue.Enqueue(dockedEvent);

        return true;
    }

    public void Cancel()
    {
        _queue.Clear();
        IsSimulationInProgress = false;
        Parcels.Clear();
    }

    public ConcurrentBag<SimulatedDto> Parcels { get; }
    
    public string DeliveryRegionId { get; private set; }
}