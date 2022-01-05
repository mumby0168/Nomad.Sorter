using MediatR;

namespace Nomad.Abstractions.Cqrs;

public interface ICommandHandler<in T> : INotificationHandler<T> where T : ICommand
{
    
}