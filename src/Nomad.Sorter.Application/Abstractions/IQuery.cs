using MediatR;

namespace Nomad.Sorter.Application.Abstractions;

public interface IQuery<out T> : IRequest<T>
{
    
}