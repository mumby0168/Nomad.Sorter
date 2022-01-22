namespace Nomad.Simulator.Models;

public record StartSimulationCommand(
    int ParcelsInSimulation,
    int InductDelayInSeconds = 10);