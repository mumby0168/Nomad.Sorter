namespace Nomad.Simulator.Services;



public interface IMessageQueuingService
{
    Queue<object> Operations { get; }
}