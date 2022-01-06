using System.Net;
using CleanArchitecture.Exceptions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.CosmosRepository;
using Nomad.Sorter.Application.Infrastructure;
using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Infrastructure.Cosmos.Items;

namespace Nomad.Sorter.Infrastructure.Cosmos.Repositories;

public class ParcelRepository : IParcelRepository
{
    private readonly IRepository<Parcel> _parcelCosmosRepository;
    private readonly IRepository<ParcelIdLookup> _parcelIdLookupCosmosRepository;

    public ParcelRepository(IRepository<Parcel> parcelCosmosRepository,
        IRepository<ParcelIdLookup> parcelIdLookupCosmosRepository)
    {
        _parcelCosmosRepository = parcelCosmosRepository;
        _parcelIdLookupCosmosRepository = parcelIdLookupCosmosRepository;
    }

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
}