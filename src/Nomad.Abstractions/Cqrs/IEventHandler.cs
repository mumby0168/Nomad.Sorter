using MediatR;

namespace Nomad.Abstractions.Cqrs;

public interface IEventHandler<in T> : INotificationHandler<T> where T : IEvent
{
    
}