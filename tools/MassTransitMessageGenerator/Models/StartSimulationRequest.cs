namespace MassTransitMessageGenerator.Models;

public record StartSimulationRequest(
    int ParcelsSimulation,
    int VehiclesCapacity);