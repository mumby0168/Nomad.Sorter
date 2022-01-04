using MediatR;

namespace Nomad.Sorter.Application.Abstractions;

public interface IEventHandler<in T> : INotificationHandler<T> where T : IEvent
{
    
}