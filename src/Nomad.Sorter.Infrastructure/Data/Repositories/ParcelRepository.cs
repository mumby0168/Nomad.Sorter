using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using CleanArchitecture.Exceptions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Extensions.Logging;
using Nomad.Sorter.Application.Infrastructure;
using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Domain.Enums;
using Nomad.Sorter.Domain.Identitifiers;
using Nomad.Sorter.Infrastructure.Data.Items;

namespace Nomad.Sorter.Infrastructure.Data.Repositories;

public class ParcelRepository : IParcelRepository
{
    private readonly ILogger<ParcelRepository> _logger;
    private readonly IRepository<Parcel> _parcelCosmosRepository;
    private readonly IRepository<ParcelLookupByParcelIdItem> _parcelIdLookupCosmosRepository;

    public ParcelRepository(ILogger<ParcelRepository> logger,
        IRepository<Parcel> parcelCosmosRepository,
        IRepository<ParcelLookupByParcelIdItem> parcelIdLookupCosmosRepository)
    {
        _logger = logger;
        _parcelCosmosRepository = parcelCosmosRepository;
        _parcelIdLookupCosmosRepository = parcelIdLookupCosmosRepository;
    }

    /// <summary>
    /// Creates a <see cref="Parcel"/>
    /// </summary>
    /// <param name="parcel">The parcel create.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the create operation.</param>
    /// <exception cref="ResourceExistsException{Parcel}"></exception>
    public async ValueTask CreateParcel(IParcel parcel, CancellationToken cancellationToken = default)
    {
        try
        {
            await _parcelCosmosRepository.CreateAsync(parcel.ToCosmosModel(), cancellationToken);
        }
        catch (CosmosException e) when (e.StatusCode is HttpStatusCode.Conflict)
        {
            throw new ResourceExistsException<Parcel>(
                $"A parcel with the ID {parcel.ParcelId} and delivery region ID {parcel.DeliveryInformation.RegionId} already exists");
        }
    }

    /// <summary>
    /// Gets a parcel by it's <see cref="ParcelId"/>
    /// </summary>
    /// <param name="parcelId">The ID of the parcel read.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the read operation.</param>
    /// <returns>An <see cref="IParcel"/></returns>
    /// <exception cref="ResourceNotFoundException{Parcel}">Occurs when the <see cref="Parcel"/> cannot be found or when the <see cref="ParcelLookupByParcelIdItem"/> cannot be found.</exception>
    /// <remarks>This method makes a double hop to read the full <see cref="Parcel"/> it first reads the <see cref="ParcelLookupByParcelIdItem"/> via a point read,
    /// before making another point read to get the full <see cref="Parcel"/>, using the delivery region ID that is defined on the <see cref="ParcelLookupByParcelIdItem"/>
    /// and the <see cref="ParcelId"/> passed to this method.</remarks>
    public async ValueTask<IParcel> GetParcel(ParcelId parcelId, CancellationToken cancellationToken = default)
    {
        string? deliveryRegionId;

        try
        {
            var parcelReference =
                await _parcelIdLookupCosmosRepository.GetAsync(parcelId.ToString(),
                    cancellationToken: cancellationToken);
            deliveryRegionId = parcelReference.DeliveryRegionId;
        }
        catch (CosmosException e) when (e.StatusCode is HttpStatusCode.NotFound)
        {
            _logger.LogError(e, "Failed to find parcel lookup with ID {ParcelId}", parcelId);

            throw new ResourceNotFoundException<Parcel>($"A parcel with the ID {parcelId} was not found");
        }

        return await GetParcel(parcelId, deliveryRegionId, cancellationToken);
    }

    /// <summary>
    /// Gets a <see cref="Parcel"/> by it's ID and delivery region ID.
    /// </summary>
    /// <param name="parcelId">The ID of the parcel to read.</param>
    /// <param name="deliveryRegionId">The delivery region ID the parcel has been assigned.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the read operation.</param>
    /// <returns>An <see cref="IParcel"/></returns>
    /// <exception cref="ResourceNotFoundException{Parcel}">Occurs when a parcel with the given <see cref="ParcelId"/> in the given delivery region ID does not exist.</exception>
    public async ValueTask<IParcel> GetParcel(ParcelId parcelId, string deliveryRegionId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _parcelCosmosRepository.GetAsync(parcelId, deliveryRegionId, cancellationToken);
        }
        catch (CosmosException e) when (e.StatusCode is HttpStatusCode.NotFound)
        {
            _logger.LogError(e,
                "Failed to find parcel with ID {ParcelId} and the delivery region ID {DeliveryRegionId}",
                parcelId, deliveryRegionId);

            throw new ResourceNotFoundException<Parcel>(
                $"A parcel with the ID {parcelId} and the delivery region ID {deliveryRegionId} was not found");
        }
    }

    public async IAsyncEnumerable<IParcel> GetParcelsWithDeliveryRegionId(
        string deliveryRegionId,
        int max,
        int chunkSize = 25,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        int collected = 0;
        bool hasMoreResults = true;
        string? token = null;

        Expression<Func<Parcel, bool>> expression = parcel =>
            parcel.PartitionKey == deliveryRegionId &&
            parcel.Status == ParcelStatus.Inducted;

        while (hasMoreResults is false || collected >= max)
        {
            var page = await _parcelCosmosRepository.PageAsync(expression, chunkSize, token, cancellationToken);

            token = page.Continuation;
            hasMoreResults = page.Continuation is not null;

            foreach (var item in page.Items)
            {
                if (collected < max)
                {
                    yield return item;   
                }
                
                collected++;
            }
        }
    }

    /// <summary>
    /// Saves any changes to the <see cref="Parcel"/>
    /// </summary>
    /// <param name="parcel">The parcel to save.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the save operation.</param>
    public async ValueTask SaveParcel(IParcel parcel, CancellationToken cancellationToken = default) =>
        await _parcelCosmosRepository.UpdateAsync(parcel.ToCosmosModel(), cancellationToken);
}