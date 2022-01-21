namespace MassTransitMessageGenerator.Models;

public class SimulatedParcel
{
    public string Id { get; set; } = default!;

    public string DeliveryRegionId { get; set; } = default!;

    public bool HasBeenDocked { get; set; } = false;

    public bool HasBeenDispatch { get; set; } = false;
}