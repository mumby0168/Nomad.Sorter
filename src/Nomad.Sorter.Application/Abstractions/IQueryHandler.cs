using MediatR;

namespace Nomad.Sorter.Application.Abstractions;

public interface IQueryHandler<in T, TReturns> : IRequestHandler<T, TReturns> where T : IQuery<TReturns>
{
    
}