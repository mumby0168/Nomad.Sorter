namespace Nomad.Simulator.Models;

public record SimulateVehicleDockingCommand(
    string DeliveryRegionId,
    string BayId,
    int Capacity = 10,
    string Postcode = "SIM10L",
    string Registration = "SI10ULA");