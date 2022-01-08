using MediatR;

namespace Nomad.Abstractions.Cqrs;

public interface IQueryHandler<in T, TReturns> : IRequestHandler<T, TReturns> where T : IQuery<TReturns>
{
    
}