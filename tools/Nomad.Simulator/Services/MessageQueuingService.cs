namespace Nomad.Simulator.Services;

public class MessageQueuingService : IMessageQueuingService
{
    public Queue<object> Operations { get; } = new();
}