using Microsoft.Azure.CosmosRepository;

namespace Nomad.Sorter.Infrastructure.Cosmos.ChangeFeed;

public interface IChangeFeedItemProcessor<T> where T : IItem
{
    ValueTask HandleAsync(T item, CancellationToken cancellationToken);
}