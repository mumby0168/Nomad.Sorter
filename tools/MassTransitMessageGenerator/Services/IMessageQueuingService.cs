using Nomad.Sorter.Application.Commands;

namespace MassTransitMessageGenerator.Services;



public interface IMessageQueuingService
{
    Queue<object> Operations { get; }
}