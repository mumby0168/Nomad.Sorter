using System.Collections.Concurrent;
using Nomad.Simulator.Models;

namespace Nomad.Simulator.Services;

public interface ISimulationService
{
    bool IsSimulationInProgress { get; }
    
    Guid SimulationId { get; }

    bool TryStart(StartSimulationCommand startSimulationCommand);

    bool DockVehicle(SimulateVehicleDockingCommand dockingCommand);

    void Cancel();
    
    ConcurrentBag<SimulatedDto> Parcels { get; }
    
    public string DeliveryRegionId { get; }
}