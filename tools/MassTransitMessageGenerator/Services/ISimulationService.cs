using System.Collections.Concurrent;
using MassTransitMessageGenerator.Models;
using Parcel = Nomad.Sorter.Domain.Entities.Parcel;

namespace MassTransitMessageGenerator.Services;

public interface ISimulationService
{
    bool IsSimulationInProgress { get; }
    
    Guid SimulationId { get; }

    bool TryStart(StartSimulationRequest startSimulationRequest);

    void Cancel();
    
    ConcurrentBag<SimulatedParcel> Parcels { get; }
    
    public string DeliveryRegionId { get; }
}