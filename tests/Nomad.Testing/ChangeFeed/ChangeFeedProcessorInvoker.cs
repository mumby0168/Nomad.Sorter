using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.ChangeFeed;
using Microsoft.Extensions.DependencyInjection;

namespace Nomad.Testing.ChangeFeed;

public class ChangeFeedProcessorInvoker<T> where T : IItem
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IRepository<T> _repository;

    public ChangeFeedProcessorInvoker(IServiceProvider serviceProvider, IRepository<T> repository)
    {
        _serviceProvider = serviceProvider;
        _repository = repository;
    }

    public async ValueTask InvokeFor(string id, string? partitionKey = null)
    {
        var data = await _repository.GetAsync(id, partitionKey);
        
        _serviceProvider
            .GetRequiredService<IItemChangeFeedProcessor<T>>()?
            .HandleAsync(data, default);
    }
}