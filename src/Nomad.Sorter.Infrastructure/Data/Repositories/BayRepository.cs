using System.Net;
using CleanArchitecture.Exceptions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Extensions.Logging;
using Nomad.Sorter.Application.Infrastructure;
using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Domain.Identitifiers;

namespace Nomad.Sorter.Infrastructure.Data.Repositories;

public class BayRepository : IBayRepository
{
    private readonly ILogger<BayRepository> _logger;
    private readonly IRepository<Bay> _bayCosmosRepository;

    public BayRepository(
        ILogger<BayRepository> logger,
        IRepository<Bay> bayCosmosRepository)
    {
        _logger = logger;
        _bayCosmosRepository = bayCosmosRepository;
    }

    public async ValueTask<IBay> GetBay(BayId bayId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _bayCosmosRepository.GetAsync(bayId, nameof(Bay), cancellationToken);
        }
        catch (CosmosException e) when (e.StatusCode is HttpStatusCode.NotFound)
        {
            _logger.LogError(e, "Bay not found with ID {BayId}", bayId.Value);
            throw new ResourceNotFoundException<Bay>($"No bay with the bay ID {bayId} was found");
        }
    }

    public async ValueTask SaveBay(IBay bay, CancellationToken cancellationToken = default) => 
        await _bayCosmosRepository.UpdateAsync(bay.ToCosmosModel(), cancellationToken);
}