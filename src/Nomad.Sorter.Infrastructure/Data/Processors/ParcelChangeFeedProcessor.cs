using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.ChangeFeed;
using Microsoft.Extensions.Logging;
using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.Enums;
using Nomad.Sorter.Infrastructure.Data.Items;

namespace Nomad.Sorter.Infrastructure.Data.Processors;

public class ParcelChangeFeedProcessor : IItemChangeFeedProcessor<Parcel>
{
    private readonly ILogger<ParcelChangeFeedProcessor> _logger;
    private readonly IRepository<ParcelLookupByParcelIdItem> _lookupRepository;

    public ParcelChangeFeedProcessor(ILogger<ParcelChangeFeedProcessor> logger, IRepository<ParcelLookupByParcelIdItem> lookupRepository)
    {
        _logger = logger;
        _lookupRepository = lookupRepository;
    }
    
    public async ValueTask HandleAsync(Parcel parcel, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Parcel change being processed for parcel with {ID}", parcel.ParcelId);
        
        if (parcel.Status is ParcelStatus.PreAdvice)
        {
            await _lookupRepository.UpdateAsync(parcel.ToParcelLookupByParcelIdItem(), cancellationToken);
            _logger.LogInformation("Parcel ID lookup created for parcel with {ID}", parcel.ParcelId);
        }
    }
}