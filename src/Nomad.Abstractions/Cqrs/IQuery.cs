using MediatR;

namespace Nomad.Abstractions.Cqrs;

public interface IQuery<out T> : IRequest<T>
{
    
}