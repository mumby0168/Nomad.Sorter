using Microsoft.Extensions.DependencyInjection;
using Nomad.Sorter.Application.Infrastructure;
using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Infrastructure.Cosmos.ChangeFeed;
using Nomad.Sorter.Infrastructure.Cosmos.ChangeFeed.Processors;
using Nomad.Sorter.Infrastructure.Cosmos.Items;
using Nomad.Sorter.Infrastructure.Cosmos.Repositories;

namespace Nomad.Sorter.Infrastructure.Cosmos;

public static class CosmosExtensions
{
    public static IServiceCollection AddCosmos(this IServiceCollection services)
    {
        services.AddCosmosRepository(x =>
        {
            x.ContainerPerItemType = true;
            x.DatabaseId = CosmosConstants.DatabaseName;

            var parcelsContainerTimeToLive = TimeSpan.FromDays(3);

            x.ContainerBuilder.Configure<Parcel>(parcelContainerOptions => parcelContainerOptions
                .WithContainer(CosmosConstants.Containers.Parcels)
                .WithPartitionKey(CosmosConstants.DefaultPartitionKey)
                .WithContainerDefaultTimeToLive(parcelsContainerTimeToLive));

            x.ContainerBuilder.Configure<ParcelIdLookup>(parcelContainerOptions => parcelContainerOptions
                .WithContainer(CosmosConstants.Containers.Parcels)
                .WithPartitionKey(CosmosConstants.DefaultPartitionKey)
                .WithContainerDefaultTimeToLive(parcelsContainerTimeToLive));
        });

        services.AddSingleton<IParcelRepository, ParcelRepository>();
        services.AddHostedService<ChangeFeedProcessorService>();
        services.AddSingleton<IChangeFeedItemProcessor<Parcel>, ParcelChangeFeedProcessor>();

        return services;
    }

    public static Parcel ToCosmosModel(this IParcel parcel) =>
        parcel as Parcel ??
        throw new InvalidOperationException(
            "a IParcel must be convertible to a Parcel in order to be stored in Cosmos DB");

    public static ParcelIdLookup ToParcelIdLookup(this IParcel parcel) =>
        new(parcel.ParcelId, parcel.DeliveryInformation.RegionId);
}