using MediatR;

namespace Nomad.Sorter.Application.Abstractions;

public interface ICommandHandler<in T> : INotificationHandler<T> where T : ICommand
{
    
}